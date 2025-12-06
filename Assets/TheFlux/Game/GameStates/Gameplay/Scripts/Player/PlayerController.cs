using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player
{
    public class PlayerController
    {
        // Change this to be not dependent on camera
        private MainCameraController mainCameraController;
        private PlayerMovementController playerMovementController;
        private PlayerMovementData playerMovementData;
        
        private PlayerView playerView;
        
        [Inject]
        public PlayerController(MainCameraController mainCameraController,
            PlayerMovementController playerMovementController)
        {
            this.mainCameraController = mainCameraController;
            this.playerMovementController = playerMovementController;
        }

        public void InitEntryPoint(PlayerView playerView, PlayerMovementData playerMovementData)
        {
            mainCameraController.SetFollowTarget(playerView.transform);
            playerMovementController.InitEntryPoint(playerMovementData, playerView);
        }


        public Transform GetTransform()
        {
            return playerView.transform;
        }


    }
}