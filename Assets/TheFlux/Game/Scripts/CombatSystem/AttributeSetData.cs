using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    [CreateAssetMenu(menuName = "GAS/AttributeSetData", fileName = "AttributeSetData")]
    public class AttributeSetData : ScriptableObject
    {
        public List<Attribute> attributes = new();

        public Dictionary<string, Attribute> InstantiateDict()
        {
            var dictionary = new Dictionary<string, Attribute>(StringComparer.OrdinalIgnoreCase);
            foreach (var attribute in attributes)
            {
                dictionary[attribute.Name.Value] = new Attribute(attribute.Name, attribute.CurrentValue);
            }
            return dictionary;
        }
    }
}