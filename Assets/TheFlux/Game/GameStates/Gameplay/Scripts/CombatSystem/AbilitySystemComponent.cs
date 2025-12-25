using System;
using System.Collections.Generic;
using System.Linq;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Attribute = TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem.Attribute;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    public class AbilitySystemComponent : ITickable
    {
        private List<GameplayAbility> GrantedAbilities = new();
        public Guid OwnerId { get; private set; }
        public GameplayTagContainer Tags { get; } = new();

        // Runtime state
        private readonly Dictionary<string, Attribute> attributes = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<ActiveEffect> activeEffects = new();

        private readonly AbilitySystemRegistry abilitySystemRegistry;

        [Inject]
        public AbilitySystemComponent(AbilitySystemRegistry abilitySystemRegistry)
        {
            this.abilitySystemRegistry = abilitySystemRegistry;
        }


        public void InitEntryPoint(Guid ownerId, List<AttributeSetData> attributeSets)
        {
            OwnerId = ownerId;
            LogService.Log($"passed ownerId: {ownerId}");
            LogService.Log($"Field OwnerId: {OwnerId}");
            LogService.Log($"HashCode during init: {GetHashCode()}");

            // Set runtime attributes from scriptable object
            foreach (var pair in attributeSets
                         .Select(set => set.InstantiateDict())
                         .SelectMany(dictionary => dictionary))
            {
                attributes[pair.Key] = pair.Value;
            }
            // Initialise Current Values
            // private void Start()
            // {
            //     foreach (var attribute in attributes.Values)
            //     {
            //         attribute.SetCurrentValue(attribute.CurrentValue, ownerId);
            //     }
            // }
        }

        public void Resume()
        {
            abilitySystemRegistry.RegisterInstance(this);
        }

        public void Pause()
        {
            abilitySystemRegistry.UnregisterInstance(this);
        }
        
        public Attribute GetAttribute(string attributeName) => attributes.GetValueOrDefault(attributeName);
        public float GetAttributeValue(string attributeName) => GetAttribute(attributeName)?.CurrentValue ?? 0f;

        public ActiveEffect FindActiveEffect(GameplayEffectSpec specification) => activeEffects.Find(
            activeEffect =>
                activeEffect.Spec.Def == specification.Def &&
                activeEffect.Spec.Instigator == specification.Instigator &&
                activeEffect.Spec.Level == specification.Level);

        public void AddActiveEffect(ActiveEffect activeEffect) => activeEffects.Add(activeEffect);

        public void Tick()
        {
            LogService.Log($"OwnerId: {OwnerId}");
            LogService.Log($"HashCode during tick: {GetHashCode()}");

            TickEffects(Time.deltaTime);
        }


        public GameplayEffectSpec MakeSpec(GameplayEffectDef definition, int level, GameObject instigator)
        {
            var specification = new GameplayEffectSpec
            {
                Def = definition,
                Level = level,
                Instigator = instigator
            };
            if (definition && definition.Modifiers != null)
            {
                foreach (var modifier in definition.Modifiers)
                {
                    specification.ResolvedMagnitudes[modifier.attributeName.Value] = modifier.magnitude.Evaluate(level);
                }
            }

            return specification;
        }

        public void RemoveEffectSpec(GameplayEffectSpec specification)
        {
            foreach (var grantedTag in specification.Def.GrantedTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (Tags.HasTag(grantedTag))
                {
                    Tags.RemoveTag(grantedTag, OwnerId);
                }
            }


            List<ActiveEffect> effectToRemove = new();
            foreach (var activeEffect in activeEffects)
            {
                if (activeEffect.Spec.Equals(specification))
                {
                    effectToRemove.Add(activeEffect);
                }
            }

            foreach (var activeEffect in effectToRemove)
            {
                activeEffects.Remove(activeEffect);
            }
        }

        public void ApplyEffectSpec(GameplayEffectSpec specification)
        {
            foreach (var requiredTag in specification.Def.RequiredTargetTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (!Tags.HasTag(requiredTag))
                {
                    return;
                }
            }

            foreach (var blockingTag in specification.Def.BlockedTargetTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (Tags.HasTag(blockingTag))
                {
                    return;
                }
            }

            switch (specification.Def.Policy)
            {
                case DurationPolicy.Instant:
                    ApplyInstantEffect(specification);
                    break;
                case DurationPolicy.Duration:
                    ApplyDurationEffect(specification);
                    break;
                case DurationPolicy.Infinite:
                    ApplyInfiniteEffect(specification);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyInstantEffect(GameplayEffectSpec specification)
        {
            ApplyModifiers(this, specification);
        }

        private void ApplyInfiniteEffect(GameplayEffectSpec specification)
        {
            // Grant tags immediately
            foreach (var grantedTag in specification.Def.GrantedTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (!Tags.HasTag(grantedTag))
                {
                    Tags.AddTag(grantedTag, OwnerId);
                }
            }

            var existingActiveEffect = FindActiveEffect(specification);
            if (existingActiveEffect != null && specification.Def.CanStack)
            {
                existingActiveEffect.stacks = Mathf.Min(existingActiveEffect.stacks + 1,
                    Mathf.Max(1, specification.Def.MaxStacks));
                if (specification.Def.RefreshDurationOnStack)
                {
                    existingActiveEffect.timeRemaining = specification.Def.GetDuration(specification.Level);
                }

                return;
            }

            var activeEffect = new ActiveEffect(specification);
            AddActiveEffect(activeEffect);
            ApplyModifiers(this, specification);
        }

        private void ApplyDurationEffect(GameplayEffectSpec specification)
        {
            foreach (var grantedTag in specification.Def.GrantedTags?.Tags ?? Array.Empty<GameplayTag>())
            {
                if (!Tags.HasTag(grantedTag))
                {
                    Tags.AddTag(grantedTag, OwnerId);
                }
            }

            var existingActiveEffect = FindActiveEffect(specification);
            if (existingActiveEffect != null && specification.Def.CanStack)
            {
                existingActiveEffect.stacks = Mathf.Min(existingActiveEffect.stacks + 1,
                    Mathf.Max(1, specification.Def.MaxStacks));
                if (specification.Def.RefreshDurationOnStack)
                {
                    existingActiveEffect.timeRemaining = specification.Def.GetDuration(specification.Level);
                }

                return;
            }

            var activeEffect = new ActiveEffect(specification);
            AddActiveEffect(activeEffect);
            ApplyModifiers(this, specification);
        }

        public void ApplyModifiers(AbilitySystemComponent target, GameplayEffectSpec spec)
        {
            foreach (var mod in spec.Def.Modifiers)
            {
                var magnitude = spec.ResolvedMagnitudes[mod.attributeName.Value];
                var attribute = target.GetAttribute(mod.attributeName.Value);
                if (attribute == null) continue;

                switch (mod.operation)
                {
                    case ModifierOp.Add:
                        attribute.SetCurrentValue(attribute.CurrentValue + magnitude, OwnerId); break;
                    case ModifierOp.Multiply:
                        attribute.SetCurrentValue(attribute.CurrentValue * magnitude, OwnerId); break;
                    case ModifierOp.Override:
                        attribute.SetCurrentValue(magnitude, OwnerId); break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void TickEffects(float deltaTime)
        {
            for (var index = activeEffects.Count - 1; index >= 0; index--)
            {
                var activeEffect = activeEffects[index];
                if (activeEffect.Spec.Def.Policy == DurationPolicy.Duration)
                {
                    activeEffect.timeRemaining -= deltaTime;
                }


                if (activeEffect.Spec.Def.IsPeriodic)
                {
                    activeEffect.periodTimer -= deltaTime;
                    if (activeEffect.periodTimer <= 0f)
                    {
                        ApplyModifiers(this, activeEffect.Spec);
                        activeEffect.periodTimer = activeEffect.Spec.Def.GetPeriod(activeEffect.Spec.Level);
                    }
                }


                if (activeEffect.IsExpired)
                {
                    foreach (var grantedTag in activeEffect.Spec.Def.GrantedTags?.Tags ?? Array.Empty<GameplayTag>())
                    {
                        Tags.RemoveTag(grantedTag, OwnerId);
                    }

                    if (activeEffect.Spec.Def.Policy.Equals(DurationPolicy.Duration) &&
                        !activeEffect.Spec.Def.IsPeriodic)
                    {
                        RemoveModifiers(activeEffect);
                    }

                    activeEffects.RemoveAt(index);
                }
            }
        }

        private void RemoveModifiers(ActiveEffect activeEffect)
        {
            foreach (var modifier in activeEffect.Spec.Def.Modifiers)
            {
                var attribute = GetAttribute(modifier.attributeName.Value);
                var magnitude = activeEffect.Spec.ResolvedMagnitudes[modifier.attributeName.Value];
                if (attribute == null) continue;

                switch (modifier.operation)
                {
                    case ModifierOp.Add:
                        attribute.SetCurrentValue(attribute.CurrentValue - magnitude, OwnerId);
                        break;
                    case ModifierOp.Multiply:
                        attribute.SetCurrentValue(attribute.CurrentValue / magnitude, OwnerId);
                        break;
                    case ModifierOp.Override:
                        attribute.SetCurrentValue(magnitude, OwnerId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // public bool TryActivateAbility(GameplayAbility ability)
        // {
        //     if (!ability) return false;
        //     if (!GrantedAbilities.Contains(ability)) return false;
        //     if (!ability.CanActivate(this)) return false;
        //     ability.Activate(this, gameObject);
        //     return true;
        // }
    }
}