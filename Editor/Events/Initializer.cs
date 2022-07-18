using System.Reflection;
using LiteNinja.SOSystems.Attributes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace LiteNinja.SOSystems.Editor
{
    [System.Serializable]
    public class Initializer
    {
        private static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Search(true);
        }

        [PostProcessScene(-1)]
        public static void OnPostprocessScene()
        {
            if (!BuildPipeline.isBuildingPlayer) return;
            Search(false);
        }

        private static void Search(bool testingInEditor)
        {
            var buildIndex = EditorSceneManager.GetActiveScene().buildIndex;
            if (!(testingInEditor || buildIndex == 0)) return;

            var scriptableObjectGuids = SearchScriptableObjects();
            var go = new GameObject("ExecutorContainer");
            var executorContainer = go.AddComponent<EventContainer>();

            foreach (var soGUID in scriptableObjectGuids)
            {
                RegisterScriptableObject(soGUID, executorContainer);
            }
            
            if (BuildPipeline.isBuildingPlayer)
            {
                // Set the objects to dirty to ensures the changes made persist.
                var scene = EditorSceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty(scene);
                EditorUtility.SetDirty(go);
                EditorUtility.SetDirty(executorContainer);
                return;
            }

            executorContainer.Init(true);
        }

        private static void RegisterScriptableObject(string guid, EventContainer eventContainer)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var getObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (getObject == null) return;

            var methods = getObject.GetType().GetMethods(bindingFlags);

            foreach (var t in methods)
            {
                var attribute = System.Attribute.GetCustomAttribute(t, typeof(EventRunAttribute)) as EventRunAttribute;
                if (attribute == null || t.GetParameters().Length != 0) continue;
                if (t.IsPrivate)
                {
                    Debug.Log($"Can only use Update on public methods. {getObject.name} has it set to private");
                    continue;
                }

                var action = t.CreateDelegate(typeof(UnityAction), getObject) as UnityAction;
                AttachEventToContainer(eventContainer, attribute, action);
            }
        }

        private static string[] SearchScriptableObjects()
        {
            if (!EditorPrefs.GetBool("SOSystems_SearchInSpecificFolders"))
                return AssetDatabase.FindAssets("t:ScriptableObject");
            var lookPath = SOUnityEventsEditorWindow.GetSearchPaths().ToArray();
            return AssetDatabase.FindAssets("t:ScriptableObject", lookPath);
        }

        private static void AttachEventToContainer(EventContainer container, EventRunAttribute lifecycle,
            UnityAction action)
        {
            var eventData = new EventData
            {
                eventType = lifecycle.EventType,
                unityEvent = new UnityEvent(),
                updateRate = lifecycle.TickDelay,
                delay = lifecycle.Delay,
                executionOrder = lifecycle.ExecutionOrder
            };

            UnityEventTools.AddPersistentListener(eventData.unityEvent, action);
            container.AddEvent(eventData);
        }
    }
}