using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Gameplay.Scripts.Commands;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.SceneInitiator
{
    public class GameplayInitiator : ISceneInitiator
    {
        public SceneType SceneType => SceneType.Gameplay;
        private readonly CommandFactory commandFactory;

        [Inject]
        public GameplayInitiator(CommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public async UniTask LoadEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (GameplayEntryData) enterDataObject;
            await commandFactory.CreateCommandAsync<EnterGameplayStateCommand>()
                .SetupEntryData(entryData)
                .Execute(cancellationTokenSource);
        }

        public async UniTask StartEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (GameplayEntryData) enterDataObject;
            await commandFactory.CreateCommandAsync<StartGameplayStateCommand>()
                .SetupEntryData(entryData)
                .Execute(cancellationTokenSource);
        }

        public UniTask InitExitPoint(CancellationTokenSource cancellationTokenSource)
        {
            return UniTask.CompletedTask;
        }
    }
}