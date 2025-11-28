using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TheFlux.Core.Scripts.Services.StateMachineService
{
    public interface IGameState
    {
        CancellationTokenSource CancellationTokenSource { get; }
        GameStateType GameStateType { get; }
        UniTask LoadAsFirstGameState(CancellationTokenSource cancellationToken);
        UniTask LoadState(CancellationTokenSource cancellationTokenSource);
        UniTask StartState(CancellationTokenSource cancellationTokenSource);
        UniTask ExitState(CancellationTokenSource cancellationTokenSource);
    }
}