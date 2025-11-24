using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.CoreInitiator;
using UnityEngine;

namespace TheFlux.Core.Scripts.Services.StateMachineService
{
    public interface IStateInitiator<T> where T : class, IInitiatorEntryData
    {
        UniTask EnterState(T stateEnterData = null);
        UniTask ExitState();
    }
}