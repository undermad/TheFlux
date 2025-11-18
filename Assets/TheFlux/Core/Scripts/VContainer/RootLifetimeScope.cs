using System;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Core.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameInitiator gameInitiator;
        [SerializeField] private LoadingScreenView loadingScreen;
        [SerializeField] private UICameraView uiCamera;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LogService>(Lifetime.Singleton);
            builder.Register<SceneService>(Lifetime.Singleton);

            builder.Register<UICameraController>(Lifetime.Singleton)
                .WithParameter(uiCamera);
            
            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .WithParameter(loadingScreen);

            builder.RegisterComponent(gameInitiator);
        }
    }
}