using System.Threading;
using Cysharp.Threading.Tasks;

using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;

using UnityEngine;

using VContainer;

namespace TheFlux.Core.Scripts
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private SceneGroup[] sceneGroups;

        private LoggerService _logService;
        private SceneService _sceneService;
        private LoadingScreenController _loadingScreenController;

        private UICameraController _uiCameraController;


        [Inject]
        public void Construct(
            LoggerService logService,
            SceneService sceneService,
            LoadingScreenController loadingScreenController,
            UICameraController uiCameraController
            )
        {
            _logService = logService;
            _sceneService = sceneService;
            _loadingScreenController = loadingScreenController;
            _uiCameraController = uiCameraController;
        }

        public void Start()
        {
            _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
        }

        private async UniTaskVoid InitEntryPoint(CancellationTokenSource cancellationToken)
        {
            _loadingScreenController.Show();
            await LoadSceneGroup(cancellationToken);
        }
        
        private async UniTask LoadSceneGroup(CancellationTokenSource cancellationTokenSource)
        {
            var progress = new LoadingProgress();
            progress.Progressed += progressValue => NotifyProgress(progressValue, cancellationTokenSource).Forget();
            await _sceneService.LoadScenes(sceneGroups[0], progress, cancellationTokenSource);
            _logService.Log("Scenes loaded");
        }
        
        private async UniTask NotifyProgress(float progress, CancellationTokenSource cancellationTokenSource)
        {
            _logService.Log($"Progress: {progress}", LogLevel.Info, LogCategory.UI);
            await _loadingScreenController.SetLoadingSlider(progress, cancellationTokenSource);
            if (progress >= 1f)
            {
                _loadingScreenController.ActivateContinueButton();
            }
        }

    }
}