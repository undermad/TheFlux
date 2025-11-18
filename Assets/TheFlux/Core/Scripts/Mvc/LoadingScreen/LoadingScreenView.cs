using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenView : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Canvas loadingCanvas;
        [SerializeField, Anywhere] private Image backgroundImage;
        [SerializeField, Anywhere] private Image loadingBar;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResetSlider()
        {
            loadingBar.fillAmount = 0;
        }

        public void AnimateSliderTo(float valueBetween0To1)
        {
            loadingBar.fillAmount = valueBetween0To1;
        }
    }
}