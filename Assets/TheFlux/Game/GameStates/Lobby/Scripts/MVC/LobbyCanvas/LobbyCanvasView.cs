using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas
{
    public class LobbyCanvasView : ValidatedMonoBehaviour
    {
        [SerializeField, Child] private Button startGameButton;


        public void Init(UnityAction startGameCallback)
        {
            startGameButton.onClick.AddListener(startGameCallback);
        }
        
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        
        
        
        
    }
}