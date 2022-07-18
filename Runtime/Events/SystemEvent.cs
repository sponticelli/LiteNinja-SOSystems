using System;
using System.Collections;
using UnityEngine;

namespace LiteNinja.SOSystems
{
    [Serializable]
    public class SystemEvent
    {
        private Action _action;
        private float _updateRate;
        private float _startDelay;
        private bool _useDelay;
        private float _timer;
        private EventContainer _container;

        public int ExecutionOrder { get; }

        public SystemEvent(Action action, float updateRate, float delay, int executionOrder,
            EventContainer container)
        {
            _action = action;
            _updateRate = updateRate;
            _startDelay = delay;
            ExecutionOrder = executionOrder;
            _container = container;
        }


        public void Run()
        {
            if (_startDelay != 0)
            {
                if (_useDelay) return;
                _useDelay = true;
                _container.StartCoroutine(DelayedExecute());
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                return;
            }

            _action.Invoke();
            _timer = _updateRate;
        }

        private IEnumerator DelayedExecute()
        {
            yield return new WaitForSeconds(_startDelay);
            _action.Invoke();
            _startDelay = 0;
        }
    }
}