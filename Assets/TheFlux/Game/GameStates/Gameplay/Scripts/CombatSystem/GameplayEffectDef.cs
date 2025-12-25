using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    [CreateAssetMenu(menuName = "GAS/GameplayEffectDef", fileName = "GameplayEffectDef")]
    public class GameplayEffectDef : ScriptableObject
    {
        public string DisplayName;
        public GameplayTagContainer GrantedTags = new();
        public GameplayTagContainer RequiredTargetTags = new();
        public GameplayTagContainer BlockedTargetTags = new();


        public DurationPolicy Policy = DurationPolicy.Instant;
        public ScalableFloat DurationSeconds = ScalableFloat.Constant(0);
        public bool IsPeriodic = false;
        public ScalableFloat Period = ScalableFloat.Constant(1);


        public bool CanStack = false;
        public int MaxStacks = 1;
        public bool RefreshDurationOnStack = true;


        public List<AttributeModifier> Modifiers = new();

        public float GetDuration(float level) => Policy == DurationPolicy.Duration ? Mathf.Max(0, DurationSeconds.Evaluate(level)) : 0f;
        public float GetPeriod(float level) => Mathf.Max(0.0001f, Period.Evaluate(level));
    }
}