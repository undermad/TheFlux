using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;

namespace TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator
{
    public class GameplayInitiator : ISceneInitiator
    {
        public SceneType SceneType => SceneType.Gameplay;
        public UniTask LoadEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (GameplayEntryData) enterDataObject;
            return UniTask.CompletedTask;
        }

        public UniTask StartEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            throw new System.NotImplementedException();
        }

        public UniTask InitExitPoint(CancellationTokenSource cancellationTokenSource)
        {
            throw new System.NotImplementedException();
        }
    }
}