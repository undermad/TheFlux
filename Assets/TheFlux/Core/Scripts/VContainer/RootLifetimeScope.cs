using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using Logger = TheFlux.Core.Scripts.Services.LogService.Logger;

namespace TheFlux.Core.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameInitiator gameInitiator;
        [SerializeField] private LoadingScreenView loadingScreen;
        [SerializeField] private UICameraView uiCamera;
        [SerializeField] private MainCameraView mainCameraView;
        [SerializeField] private LoggerConfig loggerConfig;
        [SerializeField] private ActionsView actionsView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(loggerConfig.categories);
            builder.RegisterInstance(loggerConfig);
            builder.Register<Logger>(Lifetime.Singleton).WithParameter(loggerConfig);


            builder.Register<SceneInitiatorService>(Lifetime.Singleton);
            
            builder.Register<SceneService>(Lifetime.Singleton);
            builder.Register<UICameraController>(Lifetime.Singleton)
                .WithParameter(uiCamera);

            builder.RegisterComponent(mainCameraView);
            builder.Register<MainCameraController>(Lifetime.Singleton)
                .WithParameter(mainCameraView);


            builder.RegisterComponent(actionsView);
            builder.Register<ActionsController>(Lifetime.Singleton)
                .WithParameter(actionsView);

            builder.Register<MousePositionController>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MousePositionController>();
            
            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .WithParameter(loadingScreen);
            builder.RegisterComponent(gameInitiator);
            
            builder.RegisterBuildCallback(container =>
            {
                _ = container.Resolve<Logger>();
                var actionsManager = container.Resolve<ActionsController>();
                actionsManager.Init();
                actionsManager.EnableActions();
            });
        }
    }
}