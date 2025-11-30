using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;

namespace TheFlux.Core.Scripts.Events
{
    public struct GameplayInputKeyPressed : IInputKeyPressedEvent
    {
        public ActionsType ActionType;

        public GameplayInputKeyPressed(ActionsType actionType)
        {
            ActionType = actionType;
        }
    }
}