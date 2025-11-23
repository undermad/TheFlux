using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneService;

namespace TheFlux.Core.Scripts.Services.SceneInitiatorService
{
    public class SceneInitiatorService
    {
        private readonly Dictionary<SceneType, ISceneInitiator> sceneInitiators = new();
        
        public void RegisterInitiator(ISceneInitiator sceneInitiator)
        {
            sceneInitiators.Add(sceneInitiator.SceneType, sceneInitiator);
        }

        public void UnregisterInitiator(ISceneInitiator sceneInitiator)
        {
            sceneInitiators.Remove(sceneInitiator.SceneType);
        }
        
        public async UniTask InvokeInitiatorLoadEntryPoint(SceneType sceneType, IInitiatorEntryData enterData, CancellationTokenSource cancellationTokenSource)
        {
            if (enterData == null)
            {
                return;
            }
            await sceneInitiators[sceneType].LoadEntryPoint(enterData, cancellationTokenSource);
        }
        
        public async UniTask InvokeInitiatorStartEntryPoint(SceneType sceneType, IInitiatorEntryData enterData, CancellationTokenSource cancellationTokenSource)
        {
            await sceneInitiators[sceneType].StartEntryPoint(enterData, cancellationTokenSource);
        }

        public async UniTask InvokeInitiatorExitPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource)
        {
            await sceneInitiators[sceneType].InitExitPoint(cancellationTokenSource);
        }
    }
}