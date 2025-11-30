using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.UI.MVC
{
    public class PauseCanvasView : ValidatedMonoBehaviour
    {
        [SerializeField, Child] private Button pauseButton;

        public void Setup(UnityAction onClick)
        {
            pauseButton.onClick.AddListener(onClick);
        }
    }
}