using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Game.Scripts.Initiator;
using TheFlux.Game.Scripts.States;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.Scripts.VContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // SCENE
            builder.Register<ISceneInitiator, GameInitiator>(Lifetime.Scoped);
            builder.Register<IInitiatorEntryData, GameEntryData>(Lifetime.Scoped);
            
            // STATE MACHINE
            builder.Register<LobbyState>(Lifetime.Scoped);
            builder.Register<GameplayState>(Lifetime.Scoped);
            
            
            // CALLBACK
            builder.RegisterBuildCallback(container =>
            {
                var sceneInitiator = container.Resolve<SceneInitiatorService>();
                var gameInitiatorEntryData = container.Resolve<ISceneInitiator>();
                var gameEntryData = container.Resolve<IInitiatorEntryData>();
                sceneInitiator.RegisterInitiator(gameInitiatorEntryData, gameEntryData);
            });
        }
    }
}
