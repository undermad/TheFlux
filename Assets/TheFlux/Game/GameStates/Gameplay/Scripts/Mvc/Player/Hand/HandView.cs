using KBCore.Refs;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.Hand
{
    public class HandView : ValidatedMonoBehaviour
    {
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

    }
}