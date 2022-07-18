namespace LiteNinja.SOSystems.EventRunners
{
    public class LateUpdateRunner : AEventRunner
    {
        private void LateUpdate()
        {
            Run();
        }
    }
}