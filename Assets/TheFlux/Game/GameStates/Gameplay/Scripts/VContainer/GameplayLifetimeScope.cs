using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Gameplay.Scripts.Input;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data;
using TheFlux.Game.GameStates.Gameplay.Scripts.SceneInitiator;
using TheFlux.Game.GameStates.Gameplay.Scripts.Services;
using TheFlux.Game.GameStates.Gameplay.Scripts.UI.MVC;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private PauseCanvasView pauseView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // COMMAND FACTORY
            builder.Register<CommandFactory>(Lifetime.Scoped);
            
            // SCENE
            builder.Register<ISceneInitiator, GameplayInitiator>(Lifetime.Scoped);
            builder.Register<IInitiatorEntryData, GameplayEntryData>(Lifetime.Scoped);
            
            // PLAYER
            builder.Register<PlayerFactory>(Lifetime.Scoped);
            builder.Register<PlayerMovementController>(Lifetime.Scoped)
                .As<PlayerMovementController>()
                .As<ITickable>();
            builder.Register<PlayerController>(Lifetime.Scoped);
            builder.Register<PauseCanvasController>(Lifetime.Scoped).WithParameter(pauseView);
            
            // SERVICES
            builder.Register<PauseService>(Lifetime.Scoped);
            
            // STANDALONE UNITY LIFECYCLE
            builder.RegisterEntryPoint<GameplayInputReceiver>(Lifetime.Scoped);
            
            // CALLBACK
            builder.RegisterBuildCallback(container =>
            {
                var sceneInitiatorService = container.Resolve<SceneInitiatorService>();
                var gameplayInitiator = container.Resolve<ISceneInitiator>();
                var gameplayEntryData = container.Resolve<IInitiatorEntryData>();
                sceneInitiatorService.RegisterInitiator(gameplayInitiator, gameplayEntryData);
            });
        }
    }
}