using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Core.Scripts.Services.StateMachineService;
using VContainer;

namespace TheFlux.Game.Scripts.States
{
    public class GameplayState : BaseGameState
    {
        public override GameStateType GameStateType => GameStateType.GamePlay;

        private SceneService sceneService;

        [Inject]
        public GameplayState(SceneService sceneService)
        {
            this.sceneService = sceneService;
        }

        public override async UniTask LoadState(CancellationTokenSource cancellationTokenSource)
        {
            await base.LoadState(cancellationTokenSource);
            await sceneService.LoadScenes(SceneGroupsName.Gameplay, cancellationTokenSource);
        }

        public override async UniTask StartState(CancellationTokenSource cancellationTokenSource)
        {
            await base.StartState(cancellationTokenSource);
            await sceneService.StartScenes(SceneGroupsName.Gameplay, cancellationTokenSource);
        }

        public override async UniTask ExitState(CancellationTokenSource cancellationTokenSource)
        {
            await base.ExitState(cancellationTokenSource);
            await sceneService.UnloadScenes(SceneGroupsName.Gameplay, cancellationTokenSource);
        }
    }
}