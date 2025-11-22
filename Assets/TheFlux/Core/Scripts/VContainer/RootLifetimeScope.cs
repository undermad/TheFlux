using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace TheFlux.Core.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameInitiator gameInitiator;
        [SerializeField] private LoadingScreenView loadingScreen;
        [SerializeField] private UICameraView uiCamera;
        [SerializeField] private LoggerConfig loggerConfig;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(loggerConfig.categories);
            builder.RegisterInstance(loggerConfig);
            builder.Register<LoggerService>(Lifetime.Singleton);

            builder.Register<SceneService>(Lifetime.Singleton);
            builder.Register<UICameraController>(Lifetime.Singleton)
                .WithParameter(uiCamera);
            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .WithParameter(loadingScreen);
            builder.RegisterComponent(gameInitiator);
        }
    }
}