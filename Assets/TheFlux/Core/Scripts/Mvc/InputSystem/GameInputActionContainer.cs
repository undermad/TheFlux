using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    [CreateAssetMenu(fileName = "GameInputActionContainer", menuName = "Input/GameInputActionContainer")]
    public class GameInputActionContainer : ScriptableObject
    {
        public List<GameInputAction> inputActions;
    }
}