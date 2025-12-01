using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Commands
{
    public class StartGameplayStateCommand : BaseCommand, ICommandAsync
    {
        private GameplayEntryData gameplayEntryData;
        private InputActionsController inputActionsController;

        public StartGameplayStateCommand SetupEntryData(GameplayEntryData gameplayEntryData)
        {
            this.gameplayEntryData = gameplayEntryData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            inputActionsController = ObjectResolver.Resolve<InputActionsController>();
        }

        public UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            inputActionsController.SwitchToActionMap(ActionMapType.Player);
            return UniTask.CompletedTask;
        }
    }
}