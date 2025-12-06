using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.Addressables;
using TheFlux.Game.GameStates.Gameplay.Scripts.Player.PlayerMovement.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using Object = UnityEngine.Object;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Player
{
    public class PlayerFactory : IDisposable
    {
        private const string PLAYER_VIEW_PREFAB_ADDRESS = "Player/PlayerView";
        private const string PLAYER_MOVEMENT_DATA_ADDRESS = "Player/PlayerMovementData";

        private AddressablesLoaderService addressablesLoaderService;
        private IObjectResolver objectResolver;

        [Inject]
        public PlayerFactory(IObjectResolver objectResolver, AddressablesLoaderService addressablesLoaderService)
        {
            this.objectResolver = objectResolver;
            this.addressablesLoaderService = addressablesLoaderService;
        }


        public async UniTask<PlayerController> Create(CancellationTokenSource cancellationTokenSource)
        {
            var playerViewPrefab = await addressablesLoaderService
                .LoadAsync<PlayerView>(PLAYER_VIEW_PREFAB_ADDRESS, cancellationTokenSource);
            var playerView = Object.Instantiate(playerViewPrefab, Vector3.zero, Quaternion.identity);
            
            var playerMovementData = await addressablesLoaderService
                .LoadAsync<PlayerMovementData>(PLAYER_MOVEMENT_DATA_ADDRESS, cancellationTokenSource);
            var playerController = objectResolver.Resolve<PlayerController>();
            playerController.InitEntryPoint(playerView, playerMovementData);

            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            return playerController;
        }

        public void Dispose()
        {
            addressablesLoaderService.Release(PLAYER_VIEW_PREFAB_ADDRESS);
            addressablesLoaderService.Release(PLAYER_MOVEMENT_DATA_ADDRESS);
        }
    }
}