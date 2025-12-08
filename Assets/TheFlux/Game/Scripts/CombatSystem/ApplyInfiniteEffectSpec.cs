using System;
using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    public class ApplyInfiniteEffectSpec : IApplyEffectSpec
    {
        public void ApplyEffectSpec(GameplayEffectSpec specification, AbilitySystemComponent asc)
        {
            foreach (var requiredTag in specification.Def.RequiredTargetTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (!asc.Tags.HasTag(requiredTag))
                {
                    return;
                }
            }

            foreach (var blockingTag in specification.Def.BlockedTargetTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (asc.Tags.HasTag(blockingTag))
                {
                    return;
                }
            }

            // Grant tags immediately
            foreach (var grantedTag in specification.Def.GrantedTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (asc.Tags.HasTag(grantedTag))
                {
                    return;
                }

                asc.Tags.AddTag(grantedTag, asc.OwnerId);
            }

            var existing = asc.FindActiveEffect(specification);

            if (existing != null && specification.Def.CanStack)
            {
                existing.stacks = Mathf.Min(existing.stacks + 1, Mathf.Max(1, specification.Def.MaxStacks));
                if (specification.Def.RefreshDurationOnStack)
                {
                    existing.timeRemaining = specification.Def.GetDuration(specification.Level);
                }

                return;
            }

            var activeEffect = new ActiveEffect(specification);
            asc.AddActiveEffect(activeEffect);
            asc.ApplyModifiers(asc, specification);
        }
    }
}