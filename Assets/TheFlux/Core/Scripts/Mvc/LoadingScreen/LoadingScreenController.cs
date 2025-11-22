using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.LogService;
using VContainer;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenController
    {
        private readonly LoggerService logger;
        private readonly LoadingScreenView loadingScreenView;

        [Inject]
        public LoadingScreenController(LoggerService logger, LoadingScreenView loadingScreenView)
        {
            this.logger = logger;
            this.loadingScreenView = loadingScreenView;
        }

        public void Show()
        {
            logger.Log("Showing loading screen", LogLevel.Info, LogCategory.UI);
            loadingScreenView.ResetLoadingScreen();
            loadingScreenView.AddActionToContinueButton(Hide);
            loadingScreenView.Show();
        }

        private void Hide()
        {
            logger.Log("Hiding loading screen", LogLevel.Info, LogCategory.UI);
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