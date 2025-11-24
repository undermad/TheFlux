using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;

namespace TheFlux.Core.Scripts.Services.StateMachineService
{
    public abstract class BaseGameState : IGameState
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        protected BaseGameState()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationTokenSource CancellationTokenSource => CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
        public abstract GameStateType GameStateType { get; }

        public virtual UniTask LoadState(CancellationTokenSource cancellationTokenSource)
        {
            return UniTask.CompletedTask;
        }
        
        public virtual UniTask StartState(CancellationTokenSource cancellationTokenSource)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask ExitState(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource.Cancel();
            return UniTask.CompletedTask;
        }
    }
}