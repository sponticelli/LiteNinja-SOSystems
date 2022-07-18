using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LiteNinja.SOSystems.Editor
{
    public class SOUnityEventsEditorWindow : EditorWindow
    {
        private bool _searchSpecificFolders;
        private SearchPaths _paths;

        [MenuItem("LiteNinja/Tools/Unity Events for Scriptable Objects")]
        public static void OpenWindow()
        {
            GetWindow<SOUnityEventsEditorWindow>(false, "Unity Events for SO", true);
        }

        private void OnEnable()
        {
            _searchSpecificFolders = EditorPrefs.GetBool("SOSystems_SearchInSpecificFolders");

            var searchPathList = GetSearchPaths();
            _paths = new SearchPaths()
            {
                arraySize = searchPathList?.Count ?? 0,
                pathsToSearch = searchPathList
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.TextField("Unity Events for Scriptable Objects", EditorStyles.boldLabel);

            EditorGUILayout.TextField(
                "If you want To optimize the startup time, define specific folders, such as: Assets/ScriptableObjects",
                EditorStyles.helpBox);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawPaths();

            GUI.enabled = true;
            GUILayout.EndVertical();
        }

        private void DrawPaths()
        {
            EditorGUI.BeginChangeCheck();

            _searchSpecificFolders = EditorGUILayout.Toggle("Search In Specific Folders", _searchSpecificFolders);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("SOSystems_SearchInSpecificFolders", _searchSpecificFolders);
            }

            EditorGUI.BeginChangeCheck();

            if (!_searchSpecificFolders)
            {
                GUI.enabled = false;
            }

            EditorGUI.BeginChangeCheck();

            _paths.arraySize = EditorGUILayout.IntField("Paths", _paths.arraySize);

            if (_paths.pathsToSearch.Count < _paths.arraySize)
            {
                _paths.pathsToSearch.Add("");
            }
            else if (_paths.pathsToSearch.Count > _paths.arraySize)
            {
                var pathArray = _paths.pathsToSearch.ToArray();
                Array.Resize<string>(ref pathArray, _paths.arraySize);
                _paths.pathsToSearch = pathArray.ToList();
            }

            EditorGUI.indentLevel++;
            for (var i = 0; i < _paths.arraySize; i++)
            {
                _paths.pathsToSearch[i] = EditorGUILayout.TextField("Path", _paths.pathsToSearch[i]);
            }

            if (EditorGUI.EndChangeCheck())
            {
                var savePathData = JsonUtility.ToJson(_paths);
                EditorPrefs.SetString("SOSystems_PathData", savePathData);
            }
        }

        public static List<string> GetSearchPaths()
        {
            var getPrefs = EditorPrefs.GetString("SOSystems_PathData");
            return !string.IsNullOrEmpty(getPrefs)
                ? JsonUtility.FromJson<SearchPaths>(getPrefs).pathsToSearch
                : new List<string>();
        }
    }
}