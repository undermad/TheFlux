using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Commands
{
    public class EnterGameplayStateCommand : BaseCommand, ICommandAsync
    {
        private GameplayEntryData gameplayEntryData;

        public EnterGameplayStateCommand SetupEntryData(GameplayEntryData gameplayEntryData)
        {
            this.gameplayEntryData = gameplayEntryData;
            return this;
        }
        
        
        public override void ResolveDependencies()
        {
        }

        public UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            return UniTask.CompletedTask;
        }
    }
}