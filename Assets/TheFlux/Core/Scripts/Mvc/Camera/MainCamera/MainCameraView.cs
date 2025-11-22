using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Mvc.Camera.MainCamera
{
    public class MainCameraView : ValidatedMonoBehaviour
    {
        [SerializeField, Child] private CinemachineCamera mainCinemachineCamera;
        [SerializeField, Child] private UnityEngine.Camera mainUnityCamera;

        public UnityEngine.Camera GetMainUnityCamera()
        {
            return mainUnityCamera;
        } 
        
        public void SetFollowTarget(Transform target)
        {
            mainCinemachineCamera.Follow = target;
        }
        
    }
}