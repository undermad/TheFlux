using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheFlux.Core.Scripts.Services.Addressables
{
    public class AddressablesLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle> cachedHandlesPerAddress = new();

        public async UniTask<T> LoadAsync<T>(string address, CancellationTokenSource cancellationTokenSource) where T : Object
        {
            if (cachedHandlesPerAddress.TryGetValue(address, out var cachedHandle))
            {
                return TryGetComponent<T>(address, (Object)cachedHandle.Result);
            }

            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Object>(address);
            cachedHandlesPerAddress[address] = handle;

            await handle.WithCancellation(cancellationTokenSource.Token);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return TryGetComponent<T>(address, handle.Result);
            }

            LogService.LogService.Log($"Failed to load asset at address: {address}. Reason: {handle.OperationException?.Message}");
            cachedHandlesPerAddress.Remove(address);
            return null;
        }

        public void Release(string address)
        {
            if (cachedHandlesPerAddress.TryGetValue(address, out var handle))
            {
                UnityEngine.AddressableAssets.Addressables.Release(handle);
                cachedHandlesPerAddress.Remove(address);
            }
        }

        public void ReleaseAll()
        {
            foreach (var handle in cachedHandlesPerAddress.Values)
            {
                UnityEngine.AddressableAssets.Addressables.Release(handle);
            }
            cachedHandlesPerAddress.Clear();
        }

        public bool IsLoaded(string address)
        {
            return cachedHandlesPerAddress.ContainsKey(address);
        }

        private T TryGetComponent<T>(string address, Object loadedAsset) where T : Object
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)) && loadedAsset is GameObject go)
            {
                var component = go.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }

                LogService.LogService.Log($"GameObject at address '{address}' does not have a component of type {typeof(T).Name}");
                return null;
            }

            return loadedAsset as T;
        }
    }
}
