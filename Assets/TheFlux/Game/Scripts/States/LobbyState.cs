
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Core.Scripts.Services.StateMachineService;
using VContainer;

namespace TheFlux.Game.Scripts.States
{
    public class LobbyState : BaseGameState
    {
        public override GameStateType GameStateType => GameStateType.Lobby;

        private SceneService sceneService;

        [Inject]
        public LobbyState(SceneService sceneService)
        {
            this.sceneService = sceneService;
        }

        public override async UniTask LoadState(CancellationTokenSource cancellationTokenSource)
        {
            await base.LoadState(cancellationTokenSource);
            await sceneService.LoadScenes(SceneGroupsName.Lobby, new Progress<float>(), cancellationTokenSource);
        }

        public override async UniTask ExitState(CancellationTokenSource cancellationTokenSource)
        {
            await base.ExitState(cancellationTokenSource);
        }
    }
}