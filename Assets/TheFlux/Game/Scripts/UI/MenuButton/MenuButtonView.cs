using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace TheFlux.Game.Scripts.UI.MenuButton
{
    public class MenuButtonView : ValidatedMonoBehaviour
    {
        [SerializeField, Child] private TextMeshProUGUI textMesh;

        public void SetText(string text)
        {
            textMesh.text = text;
        }
        
    }
}