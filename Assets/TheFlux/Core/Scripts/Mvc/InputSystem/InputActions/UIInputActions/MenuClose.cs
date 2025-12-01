using TheFlux.Core.Scripts.Events;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.UIInputActions
{
    [CreateAssetMenu(fileName = "MenuClose", menuName = "Input/Actions/UI/MenuClose")]
    public class MenuClose : GameInputAction
    {
        protected override void OnAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    publisher.Publish(new UIInputKeyPressed(ActionsType.UI_MENU_CLOSE));
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