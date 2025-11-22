using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public abstract class GameInputAction : ScriptableObject
    {
        [NonSerialized] private InputAction inputAction;

        public void Initialize(InputAction inputAction)
        {
            this.inputAction = inputAction;
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

        public abstract void OnAction(InputAction.CallbackContext context);
    }
}