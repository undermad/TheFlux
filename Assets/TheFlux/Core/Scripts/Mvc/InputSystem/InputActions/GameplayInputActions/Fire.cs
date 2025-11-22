using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions
{
    [CreateAssetMenu(fileName = "Fire", menuName = "Input/Actions/Fire")]
    public class Fire : GameInputAction
    {
        public override void OnAction(InputAction.CallbackContext context)
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