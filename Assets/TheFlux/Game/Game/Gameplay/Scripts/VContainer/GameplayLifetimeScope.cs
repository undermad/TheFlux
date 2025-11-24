using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Game.Game.Gameplay.Scripts.Player;
using TheFlux.Game.Game.Gameplay.Scripts.SceneInitiator;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.Game.Gameplay.Scripts.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerView playerView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // SCENE
            builder.Register<ISceneInitiator, GameplayInitiator>(Lifetime.Scoped);
            builder.Register<IInitiatorEntryData, GameplayEntryData>(Lifetime.Scoped);
            
            // PLAYER
            builder.Register<PlayerController>(Lifetime.Scoped).WithParameter(playerView);
            
            // CALLBACK
            builder.RegisterBuildCallback(container =>
            {
                var sceneInitiatorService = container.Resolve<SceneInitiatorService>();
                var gameplayInitiator = container.Resolve<ISceneInitiator>();
                var gameplayEntryData = container.Resolve<IInitiatorEntryData>();
                sceneInitiatorService.RegisterInitiator(gameplayInitiator, gameplayEntryData);
                
                
                var playerController = container.Resolve<PlayerController>();
                playerController.Setup();
            });
        }
    }
}