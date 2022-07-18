using System;
using System.Collections.Generic;
using System.Linq;
using LiteNinja.SOSystems.EventRunners;
using UnityEngine;

namespace LiteNinja.SOSystems
{
    [Serializable]
    public class EventContainer : MonoBehaviour
    {
        [SerializeField] private List<EventData> eventData = new();

        private AwakeRunner _awakeRunner;
        private StartRunner _startRunner;
        private UpdateRunner _updateRunner;
        private FixedUpdateRunner _fixedUpdateRunner;
        private LateUpdateRunner _lateUpdateRunner;
        private OnApplicationFocusRunner _onApplicationFocusRunner;
        private OnApplicationQuitRunner _onApplicationQuitRunner;

        private void Awake()
        {
            Init(false);
        }

        public void AddEvent(EventData data)
        {
            eventData.Add(data);
        }

        public void Init(bool forceAwake)
        {
            foreach (var t in eventData)
            {
                SetData(t);
            }

            SortEvents();

            if (forceAwake && _awakeRunner != null)
            {
                _awakeRunner.Awake();
            }
        }

        private void SortEvents()
        {
            SortExecutorEvents(_awakeRunner);
            SortExecutorEvents(_startRunner);
            SortExecutorEvents(_updateRunner);
            SortExecutorEvents(_fixedUpdateRunner);
            SortExecutorEvents(_lateUpdateRunner);
            SortExecutorEvents(_onApplicationFocusRunner);
            SortExecutorEvents(_onApplicationQuitRunner);
        }

        private void SortExecutorEvents(AEventRunner eventRunner)
        {
            if (eventRunner == null) return;
            eventRunner.events = eventRunner.events.OrderBy(e => e.ExecutionOrder).ToList();
        }

        private void SetData(EventData data)
        {
            var eventCount = data.unityEvent.GetPersistentEventCount();

            for (var i = 0; i < eventCount; i++)
            {
                switch (data.eventType)
                {
                    case EventType.Awake:
                        AssignEventToRunner<AwakeRunner>(_awakeRunner, data, i);
                        break;
                    case EventType.Start:
                        AssignEventToRunner<StartRunner>(_startRunner, data, i);
                        break;
                    case EventType.Update:
                        AssignEventToRunner<UpdateRunner>(_updateRunner, data, i);
                        break;
                    case EventType.FixedUpdate:
                        AssignEventToRunner<FixedUpdateRunner>(_fixedUpdateRunner, data, i);
                        break;
                    case EventType.LateUpdate:
                        AssignEventToRunner<LateUpdateRunner>(_lateUpdateRunner, data, i);
                        break;
                    case EventType.OnApplicationFocus:
                        AssignEventToRunner<OnApplicationFocusRunner>(_onApplicationFocusRunner, data, i);
                        break;
                    case EventType.OnApplicationQuit:
                        AssignEventToRunner<OnApplicationQuitRunner>(_onApplicationQuitRunner, data, i);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void AssignEventToRunner<T>(AEventRunner eventRunner, EventData data, int delegateIndex)
            where T : AEventRunner
        {
            if (eventRunner == null)
            {
                eventRunner = gameObject.AddComponent<T>();
            }

            AddEvent(eventRunner.events, new SystemEvent
            (
                data.GetDelegate(delegateIndex),
                data.updateRate,
                data.delay,
                data.executionOrder,
                this
            ));
        }

        private void AddEvent(List<SystemEvent> events, SystemEvent systemEvent)
        {
            if (systemEvent == null) return;
            events ??= new List<SystemEvent>();
            events.Add(systemEvent);
        }
    }
}