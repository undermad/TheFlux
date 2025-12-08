using UnityEngine;

namespace TheFlux.Game.Scripts.CombatSystem
{
    [CreateAssetMenu(menuName = "GAS/AttributeNameData", fileName = "AttributeNameData")]
    public class AttributeNameData : ScriptableObject
    {
        [SerializeField] private string attributeName;
        public string Value => attributeName;
    }
}