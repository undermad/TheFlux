using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.GameStates.Gameplay.Scripts.Commands;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.UI.MVC
{
    public class PauseCanvasController
    {
        private readonly PauseCanvasView pauseCanvasView;
        private readonly CommandFactory commandFactory;

        [Inject]
        public PauseCanvasController(PauseCanvasView pauseCanvasView, CommandFactory commandFactory)
        {
            this.pauseCanvasView = pauseCanvasView;
            this.pauseCanvasView.Setup(ClosePauseMenu);
            this.commandFactory = commandFactory;
        }

        private void ClosePauseMenu()
        {
            commandFactory.CreateCommandVoid<CloseMenuCommand>()
                .Execute();
        }

        public void Show()
        {
            pauseCanvasView.gameObject.SetActive(true);
        }

        public void Hide()
        {
            pauseCanvasView.gameObject.SetActive(false);
        }
        
        
        
        
    }
}