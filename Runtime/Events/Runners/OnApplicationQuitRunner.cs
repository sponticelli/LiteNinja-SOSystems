namespace LiteNinja.SOSystems.EventRunners
{
    public class OnApplicationQuitRunner : AEventRunner
    {
        public void OnApplicationQuit()
        {
            Run();
        }
    }
}