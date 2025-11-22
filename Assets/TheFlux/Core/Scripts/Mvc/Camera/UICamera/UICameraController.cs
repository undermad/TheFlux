using VContainer;

namespace TheFlux.Core.Scripts.Mvc.Camera.UICamera
{
    public class UICameraController
    {
        private readonly UICameraView uiCameraView;

        [Inject]
        public UICameraController(UICameraView uiCameraView)
        {
            this.uiCameraView = uiCameraView;
        }
    }
}