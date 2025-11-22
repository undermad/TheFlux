using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using TheFlux.Core.Scripts.Services.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Canvas loadingCanvas;
        [SerializeField, Child] private Image backgroundImage;
        [SerializeField, Child] private AnimatedSliderView loadingSliderView;
        [SerializeField, Child] private Button continueButton;

        public void AddActionToContinueButton(Action onClickAction)
        {
            continueButton.onClick.AddListener(onClickAction.Invoke);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResetLoadingScreen()
        {
            loadingSliderView.ResetSlider();
            continueButton.gameObject.SetActive(false);
        }

        public void ActivateContinueButton()
        {
            continueButton.gameObject.SetActive(true);
        }

        public async UniTask AnimateSliderTo(float valueBetween0To1, CancellationTokenSource cancellationTokenSource)
        {
            await loadingSliderView.AnimateSliderTo(valueBetween0To1, cancellationTokenSource);
        }
    }
}