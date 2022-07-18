using System;
using UnityEngine;

namespace LiteNinja.SOSystems
{
    [Serializable]
    public abstract class ATickableSystem : ScriptableObject, ITickableSystem
    {
        [SerializeField] protected ITickableSystem.Config _config;

        public ITickableSystem.Config Configuration => _config;

        
        [NonSerialized] private float _currentUpdateTime;
        [NonSerialized] private float _currentFixedUpdateTime;
        [NonSerialized] private bool _isInitialized;
        [NonSerialized] private bool _pausedUpdate;

        public void Pause(bool paused)
        {
            _pausedUpdate = paused;

            if (!paused) return;
            _currentUpdateTime = Configuration.updateDelay;
            _currentFixedUpdateTime = Configuration.fixedUpdateDelay;
        }


        public void Init()
        {
            if (_isInitialized) return;
            OnInit();
            _isInitialized = true;
        }

        public void Tick(float deltaTime)
        {
            if (_pausedUpdate)
                return;

            if (_config.updateDelay == 0)
            {
                OnTick(deltaTime);
            }
            else
            {
                if (_currentUpdateTime >= _config.updateDelay)
                {
                    OnTick(_currentUpdateTime);
                    _currentUpdateTime = 0;
                }
                else
                {
                    _currentUpdateTime += deltaTime;
                }
            }
        }

        public void FixedTick(float deltaTime)
        {
            if (_pausedUpdate)
                return;

            if (_config.fixedUpdateDelay == 0)
            {
                OnFixedTick(deltaTime);
            }
            else
            {
                if (_currentFixedUpdateTime >= _config.fixedUpdateDelay)
                {
                    OnFixedTick(_currentFixedUpdateTime);
                    _currentFixedUpdateTime = 0;
                }
                else
                {
                    _currentFixedUpdateTime += deltaTime;
                }
            }
        }

        protected abstract void OnInit();

        protected virtual void OnTick(float deltaTime)
        {
        }

        protected virtual void OnFixedTick(float deltaTime)
        {
        }
    }
}