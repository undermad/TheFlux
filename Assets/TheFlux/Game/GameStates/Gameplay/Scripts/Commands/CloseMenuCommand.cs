using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.Services;
using TheFlux.Game.GameStates.Gameplay.Scripts.UI.MVC;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Commands
{
    public class CloseMenuCommand : BaseCommand, ICommandVoid
    {
        private PauseService pauseService;
        private InputActionsController inputActionsController;
        private PauseCanvasController pauseCanvasController;

        
        public override void ResolveDependencies()
        {
            pauseService = ObjectResolver.Resolve<PauseService>();
            inputActionsController = ObjectResolver.Resolve<InputActionsController>();
            pauseCanvasController = ObjectResolver.Resolve<PauseCanvasController>();
        }

        public void Execute()
        {
            pauseService.ResumeGame();
            pauseCanvasController.Hide();
            inputActionsController.SwitchToActionMap(ActionMapType.Player);
        }
    }
}