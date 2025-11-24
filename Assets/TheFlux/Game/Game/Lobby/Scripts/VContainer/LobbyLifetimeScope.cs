using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Game.Game.Lobby.Scripts.SceneInitiator;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.Game.Lobby.Scripts.VContainer
{
    public class LobbyLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // SCENE
            builder.Register<ISceneInitiator, LobbyInitiator>(Lifetime.Scoped);
            builder.Register<IInitiatorEntryData, LobbyEntryData>(Lifetime.Scoped);
            
            // CALLBACK
            builder.RegisterBuildCallback(container =>
            {
                var sceneInitiatorService = container.Resolve<SceneInitiatorService>();
                var lobbyInitiator = container.Resolve<ISceneInitiator>();
                var lobbyEntryData = container.Resolve<IInitiatorEntryData>();
                sceneInitiatorService.RegisterInitiator(lobbyInitiator, lobbyEntryData);
            });
        }
    }
}