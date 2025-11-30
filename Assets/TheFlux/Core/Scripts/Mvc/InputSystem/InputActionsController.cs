using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TheFlux.Core.Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Object = UnityEngine.Object;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public class InputActionsController
    {
        // TO CHANGE BINDINGS FOR DIFFERENT TYPES ex Vehicles: Use InputActionName
        private readonly ActionsView actionsView;

        private readonly Dictionary<string, GameInputAction> gameplayRuntimeActions = new();
        private readonly Dictionary<string, GameInputAction> uiRuntimeActions = new();
        
        private const string InputActionName = "Player";
        
        private readonly IPublisher<IInputKeyPressedEvent> publisher;

        [Inject]
        public InputActionsController(ActionsView actionsView, IPublisher<IInputKeyPressedEvent> publisher)
        {
            this.actionsView = actionsView;
            this.publisher = publisher;
        }
        
        public void Init()
        {
            var gameplayActions = actionsView.inputActionContainer.inputActions;
            CreateAssets(gameplayActions, gameplayRuntimeActions);
            var uiActions = actionsView.uiInputActionContainer.inputActions;
            CreateAssets(uiActions, uiRuntimeActions);
        }

        private void CreateAssets(List<GameInputAction> actions, Dictionary<string, GameInputAction> runtimeActions)
        {
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
                runtime.Initialize(inputAction, publisher);
                runtimeActions.Add(runtime.name, runtime);
            }
        }

        
        // ToDo:
        // Implement this logic well
        // Clear logging messages
        public void SwitchActionsToUI()
        {
            actionsView.inputActionAsset.FindActionMap("UI").Enable();
            foreach (var action in uiRuntimeActions)
            {
                action.Value.Enable();
            }
        }

        public void EnableActions()
        {
            actionsView.inputActionAsset.FindActionMap(InputActionName).Enable();
            foreach (var action in gameplayRuntimeActions)
            {
                action.Value.Enable();
            }
        }

        public void DisableActions()
        {
            foreach (var action in gameplayRuntimeActions)
            {
                action.Value.Disable();
            }

            actionsView.inputActionAsset.FindActionMap(InputActionName).Disable();
        }

        public async UniTask WaitForAnyKeyPressed(CancellationTokenSource cancellationTokenSource)
        {
            await UniTask.WaitUntil(IsAnyKeyPressed, cancellationToken: cancellationTokenSource.Token);
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