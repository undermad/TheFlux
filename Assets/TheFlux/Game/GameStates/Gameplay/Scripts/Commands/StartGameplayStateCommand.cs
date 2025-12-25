using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Commands
{
    public class StartGameplayStateCommand : BaseCommand, ICommandAsync
    {
        private GameplayEntryData gameplayEntryData;
        private InputActionsController inputActionsController;
        private PlayerController playerController;

        public StartGameplayStateCommand SetupEntryData(GameplayEntryData gameplayEntryData)
        {
            this.gameplayEntryData = gameplayEntryData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            inputActionsController = ObjectResolver.Resolve<InputActionsController>();
            playerController = ObjectResolver.Resolve<PlayerController>();
        }

        public UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            inputActionsController.SwitchToActionMap(ActionMapType.Player);
            playerController.Resume();
            return UniTask.CompletedTask;
        }
    }
}