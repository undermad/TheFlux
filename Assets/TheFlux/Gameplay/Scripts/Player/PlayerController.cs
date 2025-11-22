using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Services.LogService;
using VContainer;

namespace TheFlux.Gameplay.Scripts.Player
{
    public class PlayerController
    {
        // Change this to be not dependent on camera
        private MainCameraController mainCameraController;
        
        private PlayerView playerView;
        
        [Inject]
        public PlayerController(MainCameraController mainCameraController, PlayerView playerView)
        {
            this.mainCameraController = mainCameraController;
            this.playerView = playerView;
        }

        public void Setup()
        {
            LogService.Log("Running PlayerController setup", LogLevel.Debug, LogCategory.Manager);
            mainCameraController.SetFollowTarget(playerView.transform);
        }
        
        
        
    }
}