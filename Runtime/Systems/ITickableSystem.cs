using System;

namespace LiteNinja.SOSystems
{
    public interface ITickableSystem : IService
    {
        [Serializable]
        public class Config
        {
            public bool isUpdatable;
            public bool isFixedUpdatable;

            public float updateDelay;
            public float fixedUpdateDelay;
        }
        
        Config Configuration { get;  }
        
        public void Init();
        public void Tick(float deltaTime);
        public void FixedTick(float deltaTime);
        public void Pause(bool paused);
    }
}

