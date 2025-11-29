using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Game.Game.Lobby.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.Game.Lobby.Scripts.VContainer
{
    public class LobbyLifetimeScope : LifetimeScope
    {
        [SerializeField] private LobbyCanvasView lobbyCanvasView;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            // COMMAND FACTORY
            builder.Register<CommandFactory>(Lifetime.Scoped);
            
            // SCENE
            builder.Register<ISceneInitiator, LobbyInitiator>(Lifetime.Scoped);
            builder.Register<IInitiatorEntryData, LobbyEntryData>(Lifetime.Scoped);
            
            // MAIN CANVAS
            builder.RegisterComponent(lobbyCanvasView);
            builder.Register<LobbyCanvasController>(Lifetime.Scoped)
                .WithParameter(lobbyCanvasView);
            
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