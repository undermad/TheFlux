using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.Hand
{
    [CreateAssetMenu(fileName = "HandData_", menuName = "Player/Hand/HandData")]
    public class HandData : ScriptableObject
    {
        public float radius = 1f;
    }
}