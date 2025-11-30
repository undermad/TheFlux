using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    [CreateAssetMenu(fileName = "InputActionContainer", menuName = "Input/InputActionContainer")]
    public class InputActionContainer : ScriptableObject
    {
        public List<GameInputAction> inputActions;
    }
}