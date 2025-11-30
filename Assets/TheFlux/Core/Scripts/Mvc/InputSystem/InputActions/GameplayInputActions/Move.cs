using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions
{
    [CreateAssetMenu(fileName = "Move", menuName = "Input/Actions/Move")]
    public class Move :  GameInputAction
    {
        protected override void OnAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    var value = context.ReadValue<Vector2>();
                    InputData.Direction = value;
                    break;
                case InputActionPhase.Canceled:
                    InputData.Direction = Vector2.zero;
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                default:
                    break;
            }
        }
    }
}