using System.Threading;
using UnityEngine;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface ICommandAsyncWithResult<T> : IBaseCommand
    {
        Awaitable<T> Execute(CancellationTokenSource cancellationTokenSource = null);
    }
}