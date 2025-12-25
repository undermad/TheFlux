using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem
{
    [Serializable]
    public class GameplayTagContainer
    {
        [SerializeField] private List<GameplayTag> tags = new();

        public IEnumerable<GameplayTag> Tags
        {
            get
            {
                foreach (var gameplayTag in tags)
                {
                    yield return gameplayTag;
                }
            }
        }

        public bool HasTag(GameplayTag tag)
        {
            foreach (var gameplayTag in Tags)
            {
                if (gameplayTag.Matches(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddTag(GameplayTag tag, Guid ownerId)
        {
            if (!HasExact(tag))
            {
                // EventBus<TagAddedEvent>.Publish(ownerId, new TagAddedEvent { Owner = ownerId, TagName = tag.Path });
                tags.Add(tag);
            }
        }

        public void RemoveTag(GameplayTag tag, Guid ownerId)
        {
            if (tags.Remove(tag))
            {
                // EventBus<TagRemovedEvent>.Publish(ownerId, new TagRemovedEvent { Owner = ownerId, TagName = tag.Path });
            }
        }

        private bool HasExact(GameplayTag tag)
        {
            foreach (var gameplayTag in tags)
            {
                if (gameplayTag.Equals(tag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}