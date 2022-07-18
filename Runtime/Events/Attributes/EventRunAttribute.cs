using System;

namespace LiteNinja.SOSystems.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventRunAttribute : System.Attribute
    {
        public EventType EventType { get; set; }
        public float Delay { get; set; }
        public float TickDelay { get; set; }
        public int ExecutionOrder { get; set; }
    }
}