using System;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    [Serializable]
    public class ActiveEffect
    {
        public GameplayEffectSpec Spec;
        public float timeRemaining;
        public float periodTimer;
        public int stacks = 1;
        public bool IsExpired => Spec.Def.Policy == DurationPolicy.Instant || (Spec.Def.Policy == DurationPolicy.Duration && timeRemaining <= 0f);

        public ActiveEffect(GameplayEffectSpec spec)
        {
            Spec = spec;
            timeRemaining = spec.Def.GetDuration(spec.Level);
            periodTimer = spec.Def.GetPeriod(spec.Level);
        }
    }
}