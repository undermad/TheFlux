using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    public class GameplayEffectSpec : IEquatable<GameplayEffectSpec>
    {
        public GameplayEffectDef Def;
        public int Level;
        public GameObject Instigator;
        public Dictionary<string, float> ResolvedMagnitudes = new(StringComparer.OrdinalIgnoreCase);


        public bool Equals(GameplayEffectSpec other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Def, other.Def) && Level == other.Level && Equals(Instigator, other.Instigator);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GameplayEffectSpec)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Def, Level, Instigator);
        }
    }
}