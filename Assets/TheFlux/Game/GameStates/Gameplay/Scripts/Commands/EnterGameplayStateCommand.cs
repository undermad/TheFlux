using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Commands
{
    public class EnterGameplayStateCommand : BaseCommand, ICommandAsync
    {
        private GameplayEntryData gameplayEntryData;
        private PlayerFactory playerFactory;

        public EnterGameplayStateCommand SetupEntryData(GameplayEntryData gameplayEntryData)
        {
            this.gameplayEntryData = gameplayEntryData;
            return this;
        }
        
        
        public override void ResolveDependencies()
        {
            playerFactory = ObjectResolver.Resolve<PlayerFactory>();
        }

        public async UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            var playerController = await playerFactory.Create(cancellationTokenSource);
        }
    }
}