using KBCore.Refs;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player
{
    public class PlayerView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Rigidbody2D rigidbody2D;

        public void Move(Vector2 direction)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + direction);
        }

        public void FlipSprite()
        {
            if (!(Mathf.Abs(InputData.Direction.x) > 0.01f)) return;
            var scale = transform.localScale;
            scale.x = InputData.Direction.x > 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }
}