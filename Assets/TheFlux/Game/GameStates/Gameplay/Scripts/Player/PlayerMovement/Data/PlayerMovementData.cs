using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "PlayerMovementData")]
    public  class PlayerMovementData : ScriptableObject
    {
        public float baseSpeed;
    }
}