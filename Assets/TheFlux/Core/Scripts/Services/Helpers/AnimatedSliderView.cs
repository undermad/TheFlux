using System.Threading;
using Cysharp.Threading.Tasks;

using DG.Tweening;

using KBCore.Refs;
using TheFlux.Core.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace TheFlux.Core.Scripts.Services.Helpers
{
    public class AnimatedSliderView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Slider slider;
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private Ease animationEase = Ease.OutQuad;

        private Tween currentAnimationTween;

        public async UniTask AnimateSliderTo(float targetValueBetween0To1, CancellationTokenSource cancellationTokenSource)
        {
            currentAnimationTween?.Kill();
            currentAnimationTween = slider.DOValue(targetValueBetween0To1, animationDuration).SetEase(animationEase);
            await currentAnimationTween.WithCancellationSafe(cancellationToken: cancellationTokenSource.Token);
        }

        public void ResetSlider()
        {
            currentAnimationTween?.Kill();
            slider.value = 0;
        }

        public float GetSliderValue()
        {
            return slider.value;
        }
    }
}