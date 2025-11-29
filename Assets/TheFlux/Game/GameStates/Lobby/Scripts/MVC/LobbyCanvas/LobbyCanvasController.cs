using VContainer;

namespace TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas
{
    public class LobbyCanvasController
    {
        private readonly LobbyCanvasView lobbyCanvasView;

        [Inject]
        public LobbyCanvasController(LobbyCanvasView lobbyCanvasView)
        {
            this.lobbyCanvasView = lobbyCanvasView;
        }

        public void ShowLobby()
        {
            lobbyCanvasView.SetActive(true);
        }

        public void HideLobby()
        {
            lobbyCanvasView.SetActive(false);
        }
    }
}