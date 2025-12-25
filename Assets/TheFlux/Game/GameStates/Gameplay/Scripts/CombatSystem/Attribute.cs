using System;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    [Serializable]
    public class Attribute
    {
        [SerializeField] private AttributeNameData name;
        [SerializeField] private float currentValue;

        public AttributeNameData Name
        {
            get => name;
            private set => name = value;
        }

        public float CurrentValue
        {
            get => currentValue;
            private set => currentValue = value;
        }

        public Attribute(AttributeNameData attributeName, float currentValue)
        {
            Name = attributeName;
            CurrentValue = currentValue;
        }

        public void SetCurrentValue(float newValue, Guid owner)
        {
            var oldValue = CurrentValue;
            CurrentValue = newValue;
            // EventBus<AttributeChangedEvent>.Publish(
            //     owner,
            //     new AttributeChangedEvent
            //     {
            //         Owner = owner,
            //         AttributeName = Name.Value,
            //         NewValue = newValue,
            //         OldValue = oldValue,
            //     });
        }
    }
}