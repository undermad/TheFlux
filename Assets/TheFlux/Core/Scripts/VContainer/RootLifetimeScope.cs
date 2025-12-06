using MessagePipe;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.Addressables;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneInitiatorService;
using TheFlux.Core.Scripts.Services.SceneService;
using TheFlux.Core.Scripts.Services.StateMachineService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;
using Logger = TheFlux.Core.Scripts.Services.LogService.Logger;

namespace TheFlux.Core.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [FormerlySerializedAs("gameInitiator")] [SerializeField] private CoreInitiator.CoreInitiator coreInitiator;
        [SerializeField] private LoadingScreenView loadingScreen;
        [SerializeField] private UICameraView uiCamera;
        [SerializeField] private MainCameraView mainCameraView;
        [SerializeField] private LoggerConfig loggerConfig;
        [SerializeField] private ActionsView actionsView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // LOGGER
            builder.RegisterInstance(loggerConfig.categories);
            builder.RegisterInstance(loggerConfig);
            builder.Register<Logger>(Lifetime.Singleton).WithParameter(loggerConfig);


            // SCENES
            builder.Register<SceneInitiatorService>(Lifetime.Singleton);
            builder.Register<SceneService>(Lifetime.Singleton);
            builder.Register<StateMachineService>(Lifetime.Singleton);
            
            // CAMERA
            builder.Register<UICameraController>(Lifetime.Singleton)
                .WithParameter(uiCamera);

            builder.RegisterComponent(mainCameraView);
            builder.Register<MainCameraController>(Lifetime.Singleton)
                .WithParameter(mainCameraView);


            // INPUT
            builder.RegisterComponent(actionsView);
            builder.Register<InputActionsController>(Lifetime.Singleton)
                .WithParameter(actionsView);

            builder.Register<MousePositionController>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MousePositionController>();
            
            
            // LOADING SCREEN
            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .WithParameter(loadingScreen);
            builder.RegisterComponent(coreInitiator);
            
            // SERVICES
            builder.Register<CommandFactory>(Lifetime.Singleton);
            builder.Register<AddressablesLoaderService>(Lifetime.Singleton);
            
            // MESSAGE PIPE
            builder.RegisterMessagePipe();
            
            // CALLBACK
            builder.RegisterBuildCallback(container =>
            {
                _ = container.Resolve<Logger>();
            });
        }
    }
}