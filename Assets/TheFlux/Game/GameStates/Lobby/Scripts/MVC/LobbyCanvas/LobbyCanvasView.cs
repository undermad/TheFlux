using UnityEngine;

namespace TheFlux.Game.GameStates.Lobby.Scripts.MVC.LobbyCanvas
{
    public class LobbyCanvasView : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}