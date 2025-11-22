using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Gameplay.Scripts.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Gameplay.Scripts.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerView playerView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerController>(Lifetime.Scoped).WithParameter(playerView);
            builder.RegisterBuildCallback(container =>
            {
                var playerController = container.Resolve<PlayerController>();
                playerController.Setup();
            });
        }
    }
}