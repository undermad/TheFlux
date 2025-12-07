using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.Addressables;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.Hand;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement.Data;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player
{
    public class PlayerFactory : IDisposable
    {
        private const string PLAYER_VIEW_PREFAB_ADDRESS = "Player/PlayerView";
        private const string PLAYER_MOVEMENT_DATA_ADDRESS = "Player/PlayerMovementData";
        
        private const string PLAYER_HAND_PREFAB_ADDRESS = "Player/HandView";
        private const string PLAYER_HAND_DATA_ADDRESS = "Player/HandData";

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
            var playerController = await CreatePlayerController(cancellationTokenSource);
            
            var handController = await CreatePlayerHandController(cancellationTokenSource);
            handController.AttachTo(playerController.GetTransform());
            
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            return playerController;
        }

        private async UniTask<PlayerController> CreatePlayerController(CancellationTokenSource cancellationTokenSource)
        {
            var playerViewPrefab = await addressablesLoaderService
                .LoadAsync<PlayerView>(PLAYER_VIEW_PREFAB_ADDRESS, cancellationTokenSource);
            var playerView = Object.Instantiate(playerViewPrefab, Vector3.zero, Quaternion.identity);
            
            var playerMovementData = await addressablesLoaderService
                .LoadAsync<PlayerMovementData>(PLAYER_MOVEMENT_DATA_ADDRESS, cancellationTokenSource);
            
            var playerController = objectResolver.Resolve<PlayerController>();
            playerController.InitEntryPoint(playerView, playerMovementData);
            
            return playerController;
        }

        private async UniTask<HandController> CreatePlayerHandController(CancellationTokenSource cancellationTokenSource)
        {
            var handViewPrefab = await addressablesLoaderService.LoadAsync<HandView>(PLAYER_HAND_PREFAB_ADDRESS, cancellationTokenSource);
            var handViw = Object.Instantiate(handViewPrefab, Vector3.zero, Quaternion.identity);
            
            var handData = await addressablesLoaderService.LoadAsync<HandData>(PLAYER_HAND_DATA_ADDRESS, cancellationTokenSource);
            
            var handController = objectResolver.Resolve<HandController>();
            handController.InitEntryPoint(handViw, handData);
            
            return handController;
        }

        public void Dispose()
        {
            addressablesLoaderService.Release(PLAYER_VIEW_PREFAB_ADDRESS);
            addressablesLoaderService.Release(PLAYER_MOVEMENT_DATA_ADDRESS);
            addressablesLoaderService.Release(PLAYER_HAND_PREFAB_ADDRESS);
            addressablesLoaderService.Release(PLAYER_HAND_DATA_ADDRESS);
        }
    }
}