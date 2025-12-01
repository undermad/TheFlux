using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TheFlux.Core.Scripts.Events;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;
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

        private ActionMapType currentActionMapType = ActionMapType.Player;
        public const string Player = "Player";
        public const string UI = "UI";

        private string MapActinMapTypeToString(ActionMapType actionMapType)
        {
            return actionMapType switch
            {
                ActionMapType.Player => Player,
                ActionMapType.UI => UI,
                _ => throw new ArgumentOutOfRangeException(nameof(actionMapType), actionMapType, null)
            };
        }
        
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

        public void SwitchToActionMap(ActionMapType actionMapType)
        {
            var currentRuntimeActions = GetGameplayActions(currentActionMapType);
            var newRuntimeActions = GetGameplayActions(actionMapType);
            DisableActions(currentActionMapType, currentRuntimeActions);
            EnableActions(actionMapType, newRuntimeActions);
            currentActionMapType = actionMapType;
        }

        private Dictionary<string, GameInputAction> GetGameplayActions(ActionMapType actionMapType)
        {
            return actionMapType switch
            {
                ActionMapType.Player => gameplayRuntimeActions,
                ActionMapType.UI => uiRuntimeActions,
                _ => throw new ArgumentOutOfRangeException(nameof(actionMapType), actionMapType, null)
            };
        }

        private void EnableActions(ActionMapType actionMapType, Dictionary<string, GameInputAction> runtimeActions)
        {
            actionsView.inputActionAsset.FindActionMap(MapActinMapTypeToString(actionMapType)).Enable();
            foreach (var action in runtimeActions)
            {
                action.Value.Enable();
            }
        }

        private void DisableActions(ActionMapType actionMapType, Dictionary<string, GameInputAction> runtimeActions)
        {
            foreach (var action in runtimeActions)
            {
                action.Value.Disable();
            }

            actionsView.inputActionAsset.FindActionMap(MapActinMapTypeToString(actionMapType)).Disable();
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