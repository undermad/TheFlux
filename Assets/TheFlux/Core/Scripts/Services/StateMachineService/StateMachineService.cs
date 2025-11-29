using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Mvc.InputSystem;
using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using VContainer;

namespace TheFlux.Core.Scripts.Services.StateMachineService
{
    public class StateMachineService
    {
        private readonly ActionsController actionsController;
        private readonly LoadingScreenController loadingScreenController;
        private IGameState _currentGameState;

        [Inject]
        public StateMachineService(LoadingScreenController loadingScreenController, ActionsController actionsController)
        {
            this.loadingScreenController = loadingScreenController;
            this.actionsController = actionsController;
        }

        public IGameState CurrentState()
        {
            return _currentGameState;
        }

        public async UniTask EnterInitialGameState(IGameState initialState,
            CancellationTokenSource cancellationTokenSource)
        {
            _currentGameState = initialState;
            await _currentGameState.LoadAsFirstGameState(cancellationTokenSource);
        }

        public void SwitchState(IGameState newState)
        {
            _ = SwitchStateAsync(newState);
        }

        private async UniTask SwitchStateAsync(IGameState newState)
        {
            try
            {
                var cancellationTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);

                if (_currentGameState == null)
                {
                    LogService.LogService.Log("No state to switch from, need to initialize a game state first!",
                        LogLevel.Error, LogCategory.Error);
                    return;
                }

                loadingScreenController.ShowWithManualLoading();
                await _currentGameState.ExitState(cancellationTokenSource);
                _ = loadingScreenController.SetLoadingSlider(0.5f, cancellationTokenSource);
                _currentGameState = newState;
                await _currentGameState.LoadState(cancellationTokenSource);
                await loadingScreenController.SetLoadingSlider(1, cancellationTokenSource);
                await loadingScreenController.ActivateWaitingAnimation();
                await actionsController.WaitForAnyKeyPressed(cancellationTokenSource);
                loadingScreenController.Hide();
                await _currentGameState.StartState(cancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.LogService.Log("Switching state operation was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogService.Log(e.Message, LogLevel.Error, LogCategory.Error);
                throw;
            }
        }
    }
}