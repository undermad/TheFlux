using System;
using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    [Serializable]
    public struct ScalableFloat
    {
        public float Base;
        public float PerLevel;
        public float Evaluate(float level) => Base + PerLevel * Mathf.Max(0, level - 1f);
        public static ScalableFloat Constant(float v) => new ScalableFloat { Base = v, PerLevel = 0f };
    }
}