using KBCore.Refs;
using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public class MousePositionController : IFixedTickable, IStartable
    {
        private readonly MainCameraController mainCameraController;
        private UnityEngine.Camera unityCamera;

        [Inject]
        public MousePositionController(MainCameraController mainCameraController)
        {
            this.mainCameraController = mainCameraController;
        }
        
        public void Start()
        {
            unityCamera = mainCameraController.GetMainCamera();
        }

        public void FixedTick()
        {
            if (!unityCamera)
            {
                return;
            }

            var worldPosition = unityCamera.ScreenToWorldPoint(new Vector3(
                InputData.PointerScreen.x,
                InputData.PointerScreen.y,
                -unityCamera.transform.position.z
            ));
            InputData.PointerWorld = worldPosition;
        }


    }
}