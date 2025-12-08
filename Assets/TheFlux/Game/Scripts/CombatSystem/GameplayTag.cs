using System;
using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    [CreateAssetMenu(menuName = "GAS/GameplayTag", fileName = "GT_")]
    public class GameplayTag : ScriptableObject, IEquatable<GameplayTag>
    {
        [SerializeField] private string path;
        public string Path => path ?? string.Empty;
        public bool Equals(GameplayTag other) => other != null && string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        public override bool Equals(object obj) => obj is GameplayTag gameplayTag && Equals(gameplayTag);
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Path);
        public bool Matches(GameplayTag other)
        {
            if (string.IsNullOrEmpty(other.Path) || string.IsNullOrEmpty(Path)) return false;
            // Parent/child matching by dot‑segments
            var mine = Path.Split('.');
            var theirs = other.Path.Split('.');
            if (theirs.Length > mine.Length) return false;
            for (var i = 0; i < theirs.Length; i++)
                if (!string.Equals(mine[i], theirs[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            return true;
        }
    }
}