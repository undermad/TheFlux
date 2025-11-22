using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using TheFlux.Core.Scripts.Services.LogService;

using UnityEngine.SceneManagement;

using VContainer;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    public class SceneService : ISceneService
    {
        private readonly LoggerService _logger;

        [Inject]
        public SceneService(LoggerService logger)
        {
            _logger = logger;
        }

        public async UniTask LoadScenes(SceneGroup group, IProgress<float> progress, CancellationTokenSource cancellationTokenSource, bool reloadDupScenes = false)
        {
            await UnloadScenes(cancellationTokenSource);

            var loadedSceneNames = new List<string>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalSceneToLoad = group.scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalSceneToLoad);
            for (var i = 0; i < totalSceneToLoad; i++)
            {
                var sceneData = group.scenes[i];
                if (!reloadDupScenes && loadedSceneNames.Contains(sceneData.Name))
                {
                    continue;
                }
                _logger.Log($"Loading scene asynchronously {sceneData.Name}");
                var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);
                operationGroup.AsyncOperations.Add(operation);
            }

            while (!operationGroup.IsDone)
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
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

        public async UniTask UnloadScenes(CancellationTokenSource cancellationTokenSource)
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
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                await UniTask.Delay(100);
            }
        }
    }
}