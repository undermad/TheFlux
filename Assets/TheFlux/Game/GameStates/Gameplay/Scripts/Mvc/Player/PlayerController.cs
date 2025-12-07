using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player
{
    public class PlayerController
    {
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