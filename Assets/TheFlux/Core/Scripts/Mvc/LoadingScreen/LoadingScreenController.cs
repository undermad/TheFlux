using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.LogService;
using VContainer;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenController
    {
        private readonly LoadingScreenView loadingScreenView;

        [Inject]
        public LoadingScreenController(LoadingScreenView loadingScreenView)
        {
            this.loadingScreenView = loadingScreenView;
        }

        public void Show()
        {
            LogService.Log("Showing loading screen", LogLevel.Info, LogCategory.UI);
            loadingScreenView.ResetLoadingScreen();
            loadingScreenView.AddActionToContinueButton(Hide);
            loadingScreenView.Show();
        }

        public void Hide()
        {
            LogService.Log("Hiding loading screen", LogLevel.Info, LogCategory.UI);
            loadingScreenView.RemoveActionFromContinueButton();
            loadingScreenView.Hide();
        }

        public async UniTask SetLoadingSlider(float valueBetween0To1, CancellationTokenSource cancellationTokenSource)
        {
            await loadingScreenView.AnimateSliderTo(valueBetween0To1, cancellationTokenSource);
        }

        public void ActivateContinueButton()
        {
            loadingScreenView.ActivateContinueButton();
        }

    }
}