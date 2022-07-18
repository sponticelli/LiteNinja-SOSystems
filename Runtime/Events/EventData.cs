using System;
using UnityEngine.Events;

namespace LiteNinja.SOSystems
{
    [Serializable]
    public class EventData
    {
        public UnityEvent unityEvent = new();
        public EventType eventType;
        public float updateRate;
        public float delay;
        public int executionOrder; //TODO should be merged with LiteNinja-Utils.ExecutionOrderAttribute

        public Action GetDelegate(int eventIndex)
        {
            var target = unityEvent.GetPersistentTarget(eventIndex);
            target.GetType().GetMethod(unityEvent.GetPersistentMethodName(eventIndex));
            return Delegate.CreateDelegate(typeof(Action), target,
                unityEvent.GetPersistentMethodName(eventIndex)) as Action;
        }
    }
}