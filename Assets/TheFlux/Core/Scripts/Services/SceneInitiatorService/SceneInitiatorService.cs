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
        private readonly Dictionary<SceneType, IInitiatorEntryData> scenesEntryData = new();
        
        public void RegisterInitiator(ISceneInitiator sceneInitiator, IInitiatorEntryData entryData)
        {
            sceneInitiators.Add(sceneInitiator.SceneType, sceneInitiator);
            scenesEntryData.Add(sceneInitiator.SceneType, entryData);
        }

        public void UnregisterInitiator(ISceneInitiator sceneInitiator, IInitiatorEntryData entryData)
        {
            sceneInitiators.Remove(sceneInitiator.SceneType);
            scenesEntryData.Remove(sceneInitiator.SceneType);
        }
        
        public async UniTask InvokeInitiatorLoadEntryPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource)
        {
            await sceneInitiators[sceneType].LoadEntryPoint(scenesEntryData[sceneType], cancellationTokenSource);
        }
        
        public async UniTask InvokeInitiatorStartEntryPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource)
        {
            await sceneInitiators[sceneType].StartEntryPoint(scenesEntryData[sceneType], cancellationTokenSource);
        }

        public async UniTask InvokeInitiatorExitPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource)
        {
            await sceneInitiators[sceneType].InitExitPoint(cancellationTokenSource);
        }
    }
}