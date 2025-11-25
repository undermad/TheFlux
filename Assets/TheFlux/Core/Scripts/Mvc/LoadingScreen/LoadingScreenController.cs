using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;
using VContainer;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenController
    {
        private readonly LoadingScreenView loadingScreenView;
        private LoadingProgress progress;
        
        public LoadingProgress LoadingProgress => progress;

        [Inject]
        public LoadingScreenController(LoadingScreenView loadingScreenView)
        {
            this.loadingScreenView = loadingScreenView;
        }

        public IProgress<float> ShowWithAutoLoading(CancellationTokenSource cancellationTokenSource)
        {
            LogService.Log("Showing loading screen", LogLevel.Info, LogCategory.UI);
            progress = new LoadingProgress();
            progress.Progressed += progressValue => NotifyProgress(progressValue, cancellationTokenSource).Forget();
            loadingScreenView.ResetLoadingScreen();
            loadingScreenView.AddActionToContinueButton(Hide);
            loadingScreenView.Show();
            return progress;
        }

        public void ShowWithManualLoading()
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
        
        private async UniTask NotifyProgress(float newProgress, CancellationTokenSource cancellationTokenSource)
        {
            LogService.Log($"Progress: {newProgress}", LogLevel.Info, LogCategory.UI);
            await SetLoadingSlider(newProgress, cancellationTokenSource);
            if (newProgress >= 1f)
            {
                ActivateContinueButton();
            }
        }

    }
}