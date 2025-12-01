using System;
using MessagePipe;
using TheFlux.Core.Scripts.Events;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions
{
    [CreateAssetMenu(fileName = "MenuOpen", menuName = "Input/Actions/MenuOpen")]

    public class MenuOpen : GameInputAction
    {
        protected override void OnAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    publisher.Publish(new GameplayInputKeyPressed(ActionsType.GAMEPLAY_MENU_OPEN));
                    break;
                case InputActionPhase.Canceled:
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Performed:
                default:
                    break;
            }
        }
    }
}