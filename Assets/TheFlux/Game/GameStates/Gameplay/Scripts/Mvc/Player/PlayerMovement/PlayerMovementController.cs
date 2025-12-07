using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement
{
    public class PlayerMovementController : ITickable
    {
        private PlayerMovementData playerMovementData;
        private PlayerView playerView;

        public void InitEntryPoint(PlayerMovementData playerMovementData, PlayerView thePlayerView)
        {
            this.playerMovementData = playerMovementData;
            playerView = thePlayerView;
        }

        public void Tick()
        {
            if (playerView == null) return;

            var currentVelocity = InputData.Direction * playerMovementData.baseSpeed;
            playerView.FlipSprite();
            playerView.Move(currentVelocity * Time.fixedDeltaTime);
        }
    }
}