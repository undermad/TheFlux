using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    [CreateAssetMenu(menuName = "GAS/Ability", fileName = "GA_")]
    public class GameplayAbility : ScriptableObject
    {
        public string displayName;
        public string description;
        public Sprite icon;

        public GameplayTagContainer GrantedTags = new();
        public GameplayTagContainer ActivationRequiredTags = new();
        public GameplayTagContainer ActivationBlockedTags = new();

        public GameplayEffectDef CostEffect;
        public GameplayEffectDef CooldownEffect;

        public List<GameplayEffectDef> EffectsToApply = new();

        public int AbilityLevel = 1;

        public virtual bool CanActivate(AbilitySystemComponent abilitySystemComponent)
        {
            if (ActivationRequiredTags != null)
            {
                foreach (var tag in ActivationRequiredTags.Tags)
                {
                    if (!abilitySystemComponent.Tags.HasTag(tag))
                    {
                        return false;
                    }
                }
            }
            
            if (ActivationBlockedTags != null)
            {
                foreach (var tag in ActivationBlockedTags.Tags)
                {
                    if (abilitySystemComponent.Tags.HasTag(tag))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual void Activate(AbilitySystemComponent targetAsc, GameObject instigator)
        {
            if (!CanActivate(targetAsc))
            {
                return;
            }
            if (CostEffect)
                targetAsc.ApplyEffectSpec(targetAsc.MakeSpec(CostEffect, AbilityLevel, instigator.gameObject));

            if (CooldownEffect)
            {
                targetAsc.ApplyEffectSpec(targetAsc.MakeSpec(CooldownEffect, AbilityLevel, instigator.gameObject));
            }
            
            foreach (var eff in EffectsToApply)
                targetAsc.ApplyEffectSpec(targetAsc.MakeSpec(eff, AbilityLevel, instigator.gameObject));
        }

        public virtual void DeActivate(AbilitySystemComponent targetAsc, GameObject instigator)
        {
            foreach (var gameplayEffectDef in EffectsToApply)
            {
                targetAsc.RemoveEffectSpec(targetAsc.MakeSpec(gameplayEffectDef, AbilityLevel, instigator.gameObject));
            }
        }
        
    }
}