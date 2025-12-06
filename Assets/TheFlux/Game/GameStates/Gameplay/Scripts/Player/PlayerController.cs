using TheFlux.Core.Scripts.Mvc.Camera.MainCamera;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player
{
    public class PlayerController
    {
        // Change this to be not dependent on camera
        private readonly PlayerMovementController playerMovementController;
        private PlayerMovementData playerMovementData;
        
        private PlayerView playerView;
        
        [Inject]
        public PlayerController(PlayerMovementController playerMovementController)
        {
            this.playerMovementController = playerMovementController;
        }

        public void InitEntryPoint(PlayerView playerView, PlayerMovementData playerMovementData)
        {
            this.playerView = playerView;
            playerMovementController.InitEntryPoint(playerMovementData, playerView);
        }

        public Transform GetTransform()
        {
            return playerView.transform;
        }
    }
}