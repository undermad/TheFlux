using System;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    public class ApplyInstantEffectSpec : IApplyEffectSpec
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

            asc.ApplyModifiers(asc, specification);
        }
    }
}