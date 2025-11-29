using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Game.GameStates.Lobby.Scripts.Commands;
using VContainer;

namespace TheFlux.Game.Game.Lobby.Scripts.SceneInitiator
{
    public class LobbyInitiator : ISceneInitiator
    {
        public SceneType SceneType => SceneType.Lobby;
        private readonly CommandFactory commandFactory;

        [Inject]
        public LobbyInitiator(CommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }


        public async UniTask LoadEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (LobbyEntryData) enterDataObject;
            await commandFactory.CreateCommandAsync<EnterLobbyStateCommand>()
                .SetEntryData(entryData)
                .Execute(cancellationTokenSource);
        }

        public async UniTask StartEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var entryData = (LobbyEntryData) enterDataObject;
            await commandFactory.CreateCommandAsync<StartLobbyStateCommand>()
                .SetEntryData(entryData)
                .Execute(cancellationTokenSource);
        }

        public UniTask InitExitPoint(CancellationTokenSource cancellationTokenSource)
        {
            throw new System.NotImplementedException();
        }
    }
}