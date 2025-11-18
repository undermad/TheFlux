using UnityEngine;

namespace TheFlux.Core.Scripts.Services.LogService
{
    public class LogService : ILogService
    {
        public void Log(string message)
        {
            Debug.unityLogger.Log(message);
        }
    }
}