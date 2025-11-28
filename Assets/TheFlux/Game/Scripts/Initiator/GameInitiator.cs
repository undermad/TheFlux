using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Core.Scripts.Services.StateMachineService;
using TheFlux.Game.Scripts.States;
using VContainer;

namespace TheFlux.Game.Scripts.Initiator
{
    public class GameInitiator : ISceneInitiator
    {
        private StateMachineService stateMachineService;
        private IObjectResolver objectResolver;

        [Inject]
        public GameInitiator(StateMachineService stateMachineService, IObjectResolver objectResolver)
        {
            this.stateMachineService = stateMachineService;
            this.objectResolver = objectResolver;
        }

        public SceneType SceneType => SceneType.Game;
        
        public async UniTask LoadEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (GameEntryData) enterDataObject;
            var lobbyState = objectResolver.Resolve<LobbyState>();
            await stateMachineService.EnterInitialGameState(lobbyState, cancellationTokenSource);  
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