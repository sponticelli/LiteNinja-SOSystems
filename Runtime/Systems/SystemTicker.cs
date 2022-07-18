using System.Collections.Generic;
using LiteNinja.Utils.Attributes;
using UnityEngine;

namespace LiteNinja.SOSystems
{
    [ExecutionOrder(-1300)]
    public class SystemTicker : MonoBehaviour
    {
        [SerializeField] protected List<ATickableSystem> systems;

        private readonly List<ITickableSystem> _updatableSystems = new();
        private readonly List<ITickableSystem> _fixedUpdatableServices = new();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _updatableSystems.Clear();
            _fixedUpdatableServices.Clear();
            foreach (var system in systems)
            {
                system.Init();
                if (system.Configuration.isFixedUpdatable)
                {
                    _fixedUpdatableServices.Add(system);
                }

                if (system.Configuration.isUpdatable)
                {
                    _updatableSystems.Add(system);
                }
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var t in _fixedUpdatableServices)
            {
                t.FixedTick(Time.fixedDeltaTime);
            }
        }

        private void Update()
        {
            foreach (var t in _updatableSystems)
            {
                t.Tick(Time.deltaTime);
            }
        }
    }
}