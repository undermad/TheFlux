using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Game.Game.Lobby.Scripts.SceneInitiator;

namespace TheFlux.Game.GameStates.Lobby.Scripts.Commands
{
    public class EnterLobbyStateCommand : BaseCommand, ICommandAsync
    {

        private LobbyEntryData lobbyEntryData;

        public EnterLobbyStateCommand SetEntryData(LobbyEntryData lobbyEntryData)
        {
            this.lobbyEntryData = lobbyEntryData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            
        }

        public UniTask Execute(CancellationTokenSource cancellationTokenSource)
        {
            // init entry points
            return UniTask.CompletedTask;
        }
    }
}