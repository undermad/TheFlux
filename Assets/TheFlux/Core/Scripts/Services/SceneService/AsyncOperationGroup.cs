using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    public readonly struct AsyncOperationGroup
    {
        public readonly List<AsyncOperation> AsyncOperations;
        
        public float Progress => AsyncOperations.Count == 0 ? 0 : AsyncOperations.Average(o => o.progress);
        public bool IsDone => AsyncOperations.All(o => o.isDone);

        public AsyncOperationGroup(int capacity)
        {
            AsyncOperations = new List<AsyncOperation>(capacity);
        }
    }
}