using System.Collections.Generic;
using LiteNinja.SOSystems.Attributes;
using UnityEngine;
using EventType = LiteNinja.SOSystems.EventType;

namespace LiteNinja.SOSystems
{
    [CreateAssetMenu(menuName = "LiteNinja/Systems/Scriptable System Ticker", fileName = "ScriptableSystemTicker",
        order = 0)]
    public class ScriptableSystemTicker : ScriptableObject
    {
        [SerializeField] protected List<ATickableSystem> systems;

        private readonly List<ITickableSystem> _updatableSystems = new();
        private readonly List<ITickableSystem> _fixedUpdatableSystems = new();

        [EventRun(EventType = EventType.Awake, ExecutionOrder = -1300)]
        public void Init()
        {
            _updatableSystems.Clear();
            _fixedUpdatableSystems.Clear();
            foreach (var system in systems)
            {
                system.Init();
                if (system.Configuration.isFixedUpdatable)
                {
                    _fixedUpdatableSystems.Add(system);
                }

                if (system.Configuration.isUpdatable)
                {
                    _updatableSystems.Add(system);
                }
            }
        }

        [EventRun(EventType = EventType.FixedUpdate, ExecutionOrder = -1300)]
        public void FixedUpdate()
        {
            foreach (var system in _fixedUpdatableSystems)
            {
                system.FixedTick(Time.fixedDeltaTime);
            }
        }

        [EventRun(EventType = EventType.Update, ExecutionOrder = -1300)]
        public void Update()
        {
            foreach (var system in _updatableSystems)
            {
                system.Tick(Time.deltaTime);
            }
        }
    }
}