using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using TheFlux.Core.Scripts.Services.SceneService;

namespace TheFlux.Core.Scripts.Services.SceneInitiatorService
{
    public interface ISceneInitiator
    {
        SceneType SceneType { get; }
        UniTask LoadEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource);
        UniTask StartEntryPoint(IInitiatorEntryData enterDataObject, CancellationTokenSource cancellationTokenSource);
        UniTask InitExitPoint(CancellationTokenSource cancellationTokenSource);
    }
}