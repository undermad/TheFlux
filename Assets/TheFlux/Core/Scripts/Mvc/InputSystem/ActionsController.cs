using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public class ActionsController
    {
        // TO CHANGE BINDINGS FOR DIFFERENT TYPES ex Vehicles: Use InputActionName
        
        private readonly ActionsView actionsView;

        private readonly List<GameInputAction> runtimeActions = new();
        private const string InputActionName = "Player";

        [Inject]
        public ActionsController(ActionsView actionsView)
        {
            this.actionsView = actionsView;
        }
        
        public void Init()
        {
            var actions = actionsView.gameInputActionContainer.inputActions;
            foreach (var asset in actions)
            {
                if (asset == null)
                {
                    Debug.unityLogger.LogWarning("Input", "Input action asset is null");
                    continue;
                }

                var inputAction = actionsView.inputActionAsset.FindAction(asset.name, throwIfNotFound: true);
                var runtime = Object.Instantiate(asset);
                runtime.name = asset.name + " (Runtime)";
                runtime.Initialize(inputAction);
                runtimeActions.Add(runtime);
            }
        }
        
        public void EnableActions()
        {
            actionsView.inputActionAsset.FindActionMap(InputActionName).Enable();
            foreach (var action in runtimeActions)
            {
                action.Enable();
            }
        }

        public void DisableActions()
        {
            foreach (var action in runtimeActions)
            {
                action.Disable();
            }

            actionsView.inputActionAsset.FindActionMap(InputActionName).Disable();
        }

        public async UniTask WaitForAnyKeyPressed(CancellationTokenSource cancellationTokenSource)
        {
            await UniTask.WaitUntil(IsAnyKeyPressed);
        }

        private bool IsAnyKeyPressed()
        {
            return
                (Keyboard.current?.anyKey.wasPressedThisFrame == true) ||
                (Mouse.current?.leftButton.wasPressedThisFrame == true) ||
                (Mouse.current?.rightButton.wasPressedThisFrame == true) ||
                (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true);
        }

    }
}