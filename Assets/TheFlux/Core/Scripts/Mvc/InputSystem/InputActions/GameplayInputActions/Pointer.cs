using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions
{
    [CreateAssetMenu(fileName = "Pointer", menuName = "Input/Actions/Pointer")]
    public class Pointer : GameInputAction
    {
        public override void OnAction(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            var value = context.ReadValue<Vector2>();
            InputData.PointerScreen = value;
        }
    }
}