using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Lobby.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas;
using VContainer;

namespace TheFlux.Game.GameStates.Lobby.Scripts.Commands
{
    public class StartLobbyStateCommand : BaseCommand, ICommandAsync
    {
        private LobbyEntryData lobbyEntryData;
        private LobbyCanvasController lobbyCanvasController;

        public StartLobbyStateCommand SetEntryData(LobbyEntryData lobbyEntryData)
        {
            this.lobbyEntryData = lobbyEntryData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            lobbyCanvasController = ObjectResolver.Resolve<LobbyCanvasController>();
        }

        public UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            lobbyCanvasController.ShowLobby();
            return UniTask.CompletedTask;
        }

    }
}