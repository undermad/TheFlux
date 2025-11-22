using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using VContainer;

namespace TheFlux.Core.Scripts.Mvc.Camera.MainCamera
{
    public class MainCameraController
    {
        private readonly MainCameraView mainCameraView;

        [Inject]
        public MainCameraController(MainCameraView mainCameraView)
        {
            this.mainCameraView = mainCameraView;
        }

        public void SetFollowTarget(Transform target)
        {
            mainCameraView.SetFollowTarget(target);
        }

        public UnityEngine.Camera GetMainCamera()
        {
            return mainCameraView.GetMainUnityCamera();
        }
    }
}