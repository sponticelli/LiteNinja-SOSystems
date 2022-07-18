using System.Collections.Generic;
using UnityEngine;

namespace LiteNinja.SOSystems.EventRunners
{
    public abstract  class AEventRunner : MonoBehaviour
    {
        public List<SystemEvent> events = new();

        protected void Run()
        {
            foreach (var systemEvent in events)
            {
                systemEvent.Run();
            }
        }
    }
}