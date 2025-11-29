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
        private readonly SceneInitiatorService.SceneInitiatorService sceneInitiatorService;

        private SceneGroup coreSceneGroup;
        private readonly Dictionary<SceneGroupsName, SceneGroup> sceneGroups = new();

        [Inject]
        public SceneService(SceneInitiatorService.SceneInitiatorService sceneInitiatorService)
        {
            this.sceneInitiatorService = sceneInitiatorService;
        }

        public void SetSceneGroups(SceneGroup coreSceneGroup, SceneGroup[] sceneGroups)
        {
            this.coreSceneGroup = coreSceneGroup;
            foreach (var sceneGroup in sceneGroups)
            {
                this.sceneGroups.Add(sceneGroup.groupName, sceneGroup);
            }
        }

        public async UniTask LoadCoreGameScenes(IProgress<float> progress,
            CancellationTokenSource cancellationTokenSource)
        {
            var loadedScenes = await LoadSceneGroup(coreSceneGroup, progress, cancellationTokenSource, false);
            await InitializeEntryPoint(loadedScenes, progress, cancellationTokenSource);
            progress.Report(1);
        }

        public async UniTask LoadScenes(
            SceneGroupsName sceneGroupsName,
            CancellationTokenSource cancellationTokenSource,
            IProgress<float> progress = null,
            bool reloadDupScenes = false)
        {
            progress ??= new Progress<float>();
            var sceneGroup = sceneGroups[sceneGroupsName];

            var loadedScenes = await LoadSceneGroup(sceneGroup, progress, cancellationTokenSource, reloadDupScenes);
            await InitializeEntryPoint(loadedScenes, progress, cancellationTokenSource);
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
                _ = sceneInitiatorService
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

        public async UniTask StartScenes(SceneGroupsName sceneGroupsName,
            CancellationTokenSource cancellationTokenSource)
        {
            var sceneGroup = sceneGroups[sceneGroupsName];
            foreach (var sceneData in sceneGroup.scenes)
            {
                await sceneInitiatorService.InvokeInitiatorStartEntryPoint(sceneData.sceneType, cancellationTokenSource);
            }
        }
        
        public async UniTask UnloadScenes(SceneGroupsName sceneGroupsName, CancellationTokenSource cancellationTokenSource)
        {
            var sceneGroup = sceneGroups[sceneGroupsName];
            var scenesNames = sceneGroup.scenes.Select(s => s.Name).ToHashSet();
            var scenesToUnload = Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .Where(scene => scene.isLoaded && scenesNames.Contains(scene.name))
                .ToList();

            var operations = scenesToUnload.Select(scene => SceneManager.UnloadSceneAsync(scene.name))
                .Where(op => op != null)
                .ToList();

            var operationGroup = new AsyncOperationGroup(operations.Count);
            operations.ForEach(op => operationGroup.AsyncOperations.Add(op));

            await UniTask.WaitUntil(() => operationGroup.IsDone, cancellationToken: cancellationTokenSource.Token);
        }

        // private async UniTask UnloadScenes(CancellationTokenSource cancellationTokenSource)
        // {
        //     var coreNames = coreSceneGroup.scenes.Select(s => s.Name).ToHashSet();
        //     // Starting from 1 should never unload Bootstrap scene
        //     var scenesToUnload = Enumerable.Range(1, SceneManager.sceneCount)
        //         .Select(SceneManager.GetSceneAt)
        //         .Where(scene => scene.isLoaded && !coreNames.Contains(scene.name))
        //         .ToList();
        //
        //     var operations = scenesToUnload.Select(scene => SceneManager.UnloadSceneAsync(scene.name))
        //         .Where(op => op != null)
        //         .ToList();
        //
        //     var operationGroup = new AsyncOperationGroup(operations.Count);
        //     operations.ForEach(op => operationGroup.AsyncOperations.Add(op));
        //
        //     await UniTask.WaitUntil(() => operationGroup.IsDone, cancellationToken: cancellationTokenSource.Token);
        // }
    }
}