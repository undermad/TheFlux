using System;
using UnityEngine.Serialization;

namespace TheFlux.Game.Scripts.CombatSystem
{
    [Serializable]
    public class AttributeModifier
    {
        public AttributeNameData attributeName;
        public ModifierOp operation = ModifierOp.Add;
        public ScalableFloat magnitude = ScalableFloat.Constant(0);
    }
}