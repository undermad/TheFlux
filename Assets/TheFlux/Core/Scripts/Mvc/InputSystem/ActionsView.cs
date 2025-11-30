using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public class ActionsView : MonoBehaviour
    {
        [FormerlySerializedAs("gameInputActionContainer")] [SerializeField] public InputActionContainer inputActionContainer;
        [SerializeField] public InputActionContainer uiInputActionContainer;
        [SerializeField] public InputActionAsset inputActionAsset;
    }
}