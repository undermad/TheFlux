using UnityEngine;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Services
{
    public class PauseService
    {
        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}