using System.Threading;
using UnityEngine;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface ICommandAsync : IBaseCommand
    {
        Awaitable Execute(CancellationTokenSource cancellationTokenSource);
    }
}