using System;
using System.Collections.Generic;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Services
{
    public class AbilitySystemRegistry : ITickable
    {
        private readonly Dictionary<Guid, AbilitySystemComponent> ownerIdToAsc = new();
        private readonly List<AbilitySystemComponent> ascToAdd = new();
        private readonly List<AbilitySystemComponent> ascToRemove = new();
        

        public void RegisterInstance(AbilitySystemComponent asc)
        {
            if (asc == null)
            {
                throw new ArgumentNullException(nameof(asc));
            }
            
            ascToAdd.Add(asc);
        }

        private void AddNewInstances()
        {
            foreach (var asc in ascToAdd)
            {
                if (!ownerIdToAsc.TryAdd(asc.OwnerId, asc))
                {
                    LogService.Log(
                        $"Component already registered {asc.OwnerId}.",
                        LogLevel.Warning, LogCategory.Manager);
                }
            }
            ascToAdd.Clear();
        }

        public void UnregisterInstance(AbilitySystemComponent asc)
        {
            if (asc == null)
            {
                throw new ArgumentNullException(nameof(asc));
            }
            
            ascToRemove.Add(asc);
        }

        private void RemoveInstances()
        {
            foreach (var asc in ascToRemove)
            {
                if (!ownerIdToAsc.Remove(asc.OwnerId))
                {
                    LogService.Log(
                        $"Component already removed or never registered: {asc.OwnerId}.",
                        LogLevel.Warning, LogCategory.Manager);
                }
            }
        }

        public void Tick()
        {
            foreach (var asc in ownerIdToAsc.Values)
            {
                asc.Tick();
            }
            AddNewInstances();
            RemoveInstances();
        }
    }
}