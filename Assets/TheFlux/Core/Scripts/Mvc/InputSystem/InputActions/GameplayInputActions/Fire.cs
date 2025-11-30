using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions
{
    [CreateAssetMenu(fileName = "Fire", menuName = "Input/Actions/Fire")]
    public class Fire : GameInputAction
    {
        protected override void OnAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InputData.IsFiring = true;
                    break;
                case InputActionPhase.Canceled:
                    InputData.IsFiring = false;
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Performed:
                default:
                    break;
            }
        }
    }
}