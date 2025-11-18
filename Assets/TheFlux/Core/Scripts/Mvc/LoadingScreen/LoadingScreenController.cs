using TheFlux.Core.Scripts.Services.LogService;
using VContainer;

namespace TheFlux.Core.Scripts.Mvc.LoadingScreen
{
    public class LoadingScreenController
    {
        private LogService _logService;
        private LoadingScreenView _loadingScreenView;

        [Inject]
        public LoadingScreenController(LogService logService, LoadingScreenView loadingScreenView)
        {
            _logService = logService;
            _loadingScreenView = loadingScreenView;
        }

        public void Show()
        {
            _logService.Log("Showing loading screen");
            _loadingScreenView.ResetSlider();
            _loadingScreenView.Show();
        }

        public void Hide()
        {
            _logService.Log("Hiding loading screen");
            _loadingScreenView.Hide();
        }

        public void SetLoadingSlider(float valueBetween0To1)
        {
            _loadingScreenView.AnimateSliderTo(valueBetween0To1);
        }
        
    }
}