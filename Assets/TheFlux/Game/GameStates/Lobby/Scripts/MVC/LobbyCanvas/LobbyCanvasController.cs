using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Scripts.Commands;
using TheFlux.Game.Scripts.States;
using VContainer;

namespace TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas
{
    public class LobbyCanvasController
    {
        private readonly LobbyCanvasView lobbyCanvasView;
        private readonly CommandFactory commandFactory;
        private readonly IObjectResolver objectResolver;

        [Inject]
        public LobbyCanvasController(LobbyCanvasView lobbyCanvasView, CommandFactory commandFactory,
            IObjectResolver objectResolver)
        {
            this.lobbyCanvasView = lobbyCanvasView;
            this.commandFactory = commandFactory;
            this.objectResolver = objectResolver;
        }

        public void ShowLobby()
        {
            lobbyCanvasView.SetActive(true);
            lobbyCanvasView.Init(() =>
            {
                commandFactory.CreateCommandVoid<SwitchStateCommand>()
                    .SetNewGameState(objectResolver.Resolve<GameplayState>())
                    .Execute();
            });
        }

        public void HideLobby()
        {
            lobbyCanvasView.SetActive(false);
        }
    }
}