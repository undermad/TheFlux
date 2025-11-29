using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface ICommandAsync : IBaseCommand
    {
        UniTask Execute(CancellationTokenSource cancellationTokenSource);
    }
}