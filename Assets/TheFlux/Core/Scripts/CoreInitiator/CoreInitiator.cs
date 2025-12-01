using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;
using UnityEngine;
using VContainer;

namespace TheFlux.Core.Scripts.CoreInitiator
{
    public class CoreInitiator : MonoBehaviour
    {
        [SerializeField] private SceneGroup coreSceneGroup;
        [SerializeField] private SceneGroup[] sceneGroups;
        private SceneService _sceneService;
        private LoadingScreenController _loadingScreenController;
        private UICameraController _uiCameraController;
        private InputActionsController inputActionsController;


        [Inject]
        public void Construct(
            SceneService sceneService,
            LoadingScreenController loadingScreenController,
            UICameraController uiCameraController,
            InputActionsController inputActionsController
        )
        {
            _sceneService = sceneService;
            _loadingScreenController = loadingScreenController;
            _uiCameraController = uiCameraController;
            this.inputActionsController = inputActionsController;
        }

        public void Start()
        {
            _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
        }

        private async UniTaskVoid InitEntryPoint(CancellationTokenSource cancellationToken)
        {
            try
            {
                var loadingProgress = _loadingScreenController.ShowWithAutoLoading(cancellationToken);
                InitialiseServices();
                await LoadSceneGroup(loadingProgress, cancellationToken);
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

        private void InitialiseServices()
        {
            _sceneService.SetSceneGroups(coreSceneGroup, sceneGroups);
            inputActionsController.Init();
            inputActionsController.SwitchToActionMap(ActionMapType.UI);
        }

        private async UniTask LoadSceneGroup(IProgress<float> loadingProgress, CancellationTokenSource cancellationTokenSource)
        {
            await _sceneService.LoadCoreGameScenes(loadingProgress, cancellationTokenSource);
            LogService.Log("Scenes loaded");
            await inputActionsController.WaitForAnyKeyPressed(cancellationTokenSource);
            _loadingScreenController.Hide();
            await _sceneService.StartScenes(SceneGroupsName.Lobby, cancellationTokenSource);
        }

    }
}