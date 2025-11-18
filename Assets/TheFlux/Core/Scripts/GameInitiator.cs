using System;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.Camera.UICamera;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Core.Scripts.Services.SceneService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Core.Scripts
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private SceneGroup[] sceneGroups;
        
        private LogService _logService;
        private SceneService _sceneService;
        private LoadingScreenController _loadingScreenController;
        
        private UICameraController _uiCameraController;


        [Inject]
        public void Construct(
            LogService logService,
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
            UniTask.Void(InitEntryPoint);
        }

        private async UniTaskVoid InitEntryPoint()
        {
            _loadingScreenController.Show();
            _loadingScreenController.SetLoadingSlider(0.5f);
            await LoadSceneGroup();
            _loadingScreenController.SetLoadingSlider(1);
            _loadingScreenController.Hide();
        }

        private void NotifyProgress(float progress)
        {
            _loadingScreenController.SetLoadingSlider(progress);
        }

        private async UniTask LoadSceneGroup()
        {
            var progress = new LoadingProgress();
            progress.Progressed += NotifyProgress;
            await _sceneService.LoadScenes(sceneGroups[0], progress);
            _logService.Log("Scenes loaded");
        }
    }
}
