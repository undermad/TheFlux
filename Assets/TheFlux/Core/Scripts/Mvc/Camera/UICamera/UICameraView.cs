using KBCore.Refs;
using UnityEngine;

namespace TheFlux.Core.Scripts.Mvc.Camera.UICamera
{
    public class UICameraView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private UnityEngine.Camera _uiCamera;
    }
}