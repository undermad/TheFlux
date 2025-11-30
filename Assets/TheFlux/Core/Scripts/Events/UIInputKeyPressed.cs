using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.UIInputActions;

namespace TheFlux.Core.Scripts.Events
{
    public struct UIInputKeyPressed : IInputKeyPressedEvent
    {
        public ActionsType ActionType;

        public UIInputKeyPressed(ActionsType actionType)
        {
            ActionType = actionType;
        }
    }
}