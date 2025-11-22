using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem.InputActions
{
    [CreateAssetMenu(fileName = "Look", menuName = "Input/Actions/Look")]
    public class Look : GameInputAction
    {
        public override void OnAction(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            InputData.Look = value;
        }
    }
}