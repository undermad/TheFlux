using VContainer;

namespace TheFlux.Core.Scripts.Mvc.Camera.UICamera
{
    public class UICameraController
    {
        private UICameraView _uiCameraView;

        [Inject]
        public UICameraController(UICameraView uiCameraView)
        {
            _uiCameraView = uiCameraView;
        }
    }
}