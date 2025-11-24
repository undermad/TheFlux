using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using VContainer;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    public class SceneService
    {
        private SceneInitiatorService.SceneInitiatorService initiatorService;
        private Dictionary<SceneGroupsName, SceneGroup> sceneGroups = new();

        [Inject]
        public SceneService(SceneInitiatorService.SceneInitiatorService initiatorService)
        {
            this.initiatorService = initiatorService;
        }

        public void SetSceneGroups(SceneGroup[] sceneGroups)
        {
            foreach (var sceneGroup in sceneGroups)
            {
                this.sceneGroups.Add(sceneGroup.groupNameName, sceneGroup);
            }
        }


        public async UniTask LoadScenes(
            SceneGroupsName sceneGroupsName,
            IProgress<float> progress,
            CancellationTokenSource cancellationTokenSource,
            bool reloadDupScenes = false)
        {
            var sceneGroup = sceneGroups[sceneGroupsName];
            
            // ToDo 
            // Handle Scene Unloading here - make sure those are efficient.
            // await UnloadScenes(cancellationTokenSource);
            var loadedScenes = await LoadSceneGroup(sceneGroup, progress, cancellationTokenSource, reloadDupScenes);
            await InitializeEntryPoint(loadedScenes, progress, cancellationTokenSource);

            progress.Report(1);
        }

        private async UniTask<List<SceneData>> LoadSceneGroup(
            SceneGroup group,
            IProgress<float> progress,
            CancellationTokenSource cancellationTokenSource,
            bool reloadDupScenes)
        {
            var loadedSceneNames = new List<string>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
            }

            var loadedScenes = new List<SceneData>();
            var totalSceneToLoad = group.scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalSceneToLoad);
            for (var i = 0; i < totalSceneToLoad; i++)
            {
                var sceneData = group.scenes[i];
                if (!reloadDupScenes && loadedSceneNames.Contains(sceneData.Name))
                {
                    continue;
                }

                LogService.LogService.Log($"Loading scene asynchronously {sceneData.Name}");
                var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);
                operationGroup.AsyncOperations.Add(operation);
                loadedScenes.Add(sceneData);
            }

            while (!operationGroup.IsDone)
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                progress.Report(operationGroup.Progress * 0.2f);
                await UniTask.Delay(100);
            }

            return loadedScenes;
        }

        private async UniTask InitializeEntryPoint(
            List<SceneData> loadedScenes,
            IProgress<float> progress,
            CancellationTokenSource cancellationTokenSource)
        {
            var completed = 0;
            var loadedScenesCount = loadedScenes.Count;
            var semaphore = new SemaphoreSlim(1, 1);
            for (var i = 0; i < loadedScenesCount; i++)
            {
                var sceneData = loadedScenes[i];
                _ = initiatorService
                    // Add Entry Data class here!
                    .InvokeInitiatorLoadEntryPoint(sceneData.sceneType, cancellationTokenSource)
                    .ContinueWith(async () =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            completed++;
                            progress.Report((float)completed / loadedScenesCount * 0.8f + 0.2f);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });
            }

            while (completed != loadedScenesCount)
            {
                await UniTask.Delay(100);
            }
            await UniTask.Delay(100);
        }

        private async UniTask UnloadScenes(CancellationTokenSource cancellationTokenSource)
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
            foreach (var operation in scenesToUnload.Select(SceneManager.UnloadSceneAsync)
                         .Where(operation => operation != null))
            {
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