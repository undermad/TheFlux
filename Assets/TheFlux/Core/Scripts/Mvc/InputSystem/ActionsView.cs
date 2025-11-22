using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public class ActionsView : MonoBehaviour
    {
        [SerializeField] public GameInputActionContainer gameInputActionContainer;
        [SerializeField] public InputActionAsset inputActionAsset;
    }
}