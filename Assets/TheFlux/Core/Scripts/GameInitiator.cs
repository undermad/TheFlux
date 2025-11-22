using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.InputSystem;
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
        private SceneService _sceneService;
        private LoadingScreenController _loadingScreenController;
        private UICameraController _uiCameraController;
        private ActionsController actionsController;


        [Inject]
        public void Construct(
            SceneService sceneService,
            LoadingScreenController loadingScreenController,
            UICameraController uiCameraController,
            ActionsController actionsController
        )
        {
            _sceneService = sceneService;
            _loadingScreenController = loadingScreenController;
            _uiCameraController = uiCameraController;
            this.actionsController = actionsController;
        }

        public void Start()
        {
            _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
        }

        private async UniTaskVoid InitEntryPoint(CancellationTokenSource cancellationToken)
        {
            try
            {
                _loadingScreenController.Show();
                await LoadSceneGroup(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation InitEntryPoint in GameInitiator cancelled.");
            }
            catch (Exception e)
            {
                LogService.Log(e.Message, LogLevel.Error, LogCategory.Error);
            }
        }

        private async UniTask LoadSceneGroup(CancellationTokenSource cancellationTokenSource)
        {
            var progress = new LoadingProgress();
            progress.Progressed += progressValue => NotifyProgress(progressValue, cancellationTokenSource).Forget();
            await _sceneService.LoadScenes(sceneGroups[0], progress, cancellationTokenSource);
            LogService.Log("Scenes loaded");
        }

        private async UniTask NotifyProgress(float progress, CancellationTokenSource cancellationTokenSource)
        {
            LogService.Log($"Progress: {progress}", LogLevel.Info, LogCategory.UI);
            await _loadingScreenController.SetLoadingSlider(progress, cancellationTokenSource);
            if (progress >= 1f)
            {
                _loadingScreenController.ActivateContinueButton();
            }
        }
    }
}