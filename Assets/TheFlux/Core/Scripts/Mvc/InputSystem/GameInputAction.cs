using System;
using MessagePipe;
using TheFlux.Core.Scripts.Events;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public abstract class GameInputAction : ScriptableObject
    {
        [NonSerialized] private InputAction inputAction;
        [SerializeField] private ActionsType actionsType;
        
        protected IPublisher<IInputKeyPressedEvent> publisher;

        public void Initialize(InputAction inputAction, IPublisher<IInputKeyPressedEvent> publisher)
        {
            this.inputAction = inputAction;
            this.publisher = publisher;
        }

        public void Enable()
        {
            if (inputAction == null)
            {
                Debug.LogWarning($"{name}: Enable() called before Initialize().");
                return;
            }

            inputAction.started += OnAction;
            inputAction.performed += OnAction;
            inputAction.canceled += OnAction;
        }

        public void Disable()
        {
            if (inputAction == null) return;

            inputAction.started -= OnAction;
            inputAction.performed -= OnAction;
            inputAction.canceled -= OnAction;
        }

        protected abstract void OnAction(InputAction.CallbackContext context);
    }
}