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
        [SerializeField, Child] private SpinnerView spinnerView;

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
            spinnerView.SetActive(false);
        }

        public async UniTask AnimateSliderTo(float valueBetween0To1, CancellationTokenSource cancellationTokenSource)
        {
            await loadingSliderView.AnimateSliderTo(valueBetween0To1, cancellationTokenSource);
        }

        public void ActivateSpinner()
        {
            spinnerView.SetActive(true);
        }

        public float GetSliderValue()
        {
            return loadingSliderView.GetSliderValue();
        }
    }
}