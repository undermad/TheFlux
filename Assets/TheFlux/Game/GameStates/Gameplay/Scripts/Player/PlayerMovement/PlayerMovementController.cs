using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement
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
            var isNull = playerView == null ? "IsNull" : "Is not null";
            LogService.Log($"Ticking player view: {isNull}");
            if (playerView == null) return;

            var currentVelocity = InputData.Direction * playerMovementData.baseSpeed;
            playerView.FlipSprite();
            playerView.Move(currentVelocity * Time.fixedDeltaTime);
        }
    }
}