using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    public class SceneService : ISceneService
    {
        private ILogService _logger;

        public SceneService(LogService.LogService logger)
        {
            _logger = logger;
            _logger.Log("SceneLoader constructed");
        }

        public async UniTask LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            await UnloadScenes();
            
            var loadedSceneNames = new List<string>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalSceneToLoad = group.Scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalSceneToLoad);
            for (var i = 0; i < totalSceneToLoad; i++)
            {
                var sceneData = group.Scenes[i];
                if (!reloadDupScenes && loadedSceneNames.Contains(sceneData.Name))
                {
                    continue;
                }
                
                Debug.Log($"Loading scene {sceneData.Name}");

                await Task.Delay(TimeSpan.FromSeconds(2.5d));

                var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);
                operationGroup.AsyncOperations.Add(operation);
            }

            while (!operationGroup.IsDone)
            {
                progress.Report(operationGroup.Progress);
                await UniTask.Delay(100);
            }
            progress.Report(operationGroup.Progress);

            var activeScene = SceneManager.GetSceneByName(group.FindSceneNameByType(SceneType.ActiveScene));
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
        }

        public async UniTask UnloadScenes()
        {
            var scenesToUnload = new List<string>();
            for (var i = 1; i < SceneManager.sceneCount; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                var loadedSceneName = loadedScene.name;
                if (!loadedScene.isLoaded || loadedSceneName.Equals("Bootstrap"))
                {
                    continue;
                }
                scenesToUnload.Add(loadedSceneName);
            }
            
            var operationGroup = new AsyncOperationGroup(scenesToUnload.Count);
            foreach (var scene in scenesToUnload)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null)
                {
                    continue;
                }
                operationGroup.AsyncOperations.Add(operation);
            }
            
            while (!operationGroup.IsDone)
            {
                await UniTask.Delay(100);
            }
        }
    }
}