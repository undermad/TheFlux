using TheFlux.Core.Scripts.Mvc.InputSystem;
using UnityEngine;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.Hand
{
    public class HandController : IFixedTickable
    {
        private HandView handView;
        private HandData handData;
        private Transform attachedTo;
        
        private Vector2 direction;

        public void InitEntryPoint(HandView handView, HandData handData)
        {
            this.handView = handView;
            this.handData = handData;
        }

        public void AttachTo(Transform transform)
        {
            attachedTo = transform;
        }


        public void FixedTick()
        {
            if(attachedTo == null) return;
            
            direction = (InputData.PointerWorld - (Vector2) attachedTo.position).normalized;
            var position = (Vector2)attachedTo.position + direction * handData.radius;
            handView.SetPosition(position);

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var quaternion = Quaternion.Euler(0f, 0f, angle - 90f);
            handView.SetRotation(quaternion);

            // ?? 
            InputData.HandPosition = position;
            InputData.HandRotation = quaternion;
        }
    }
}