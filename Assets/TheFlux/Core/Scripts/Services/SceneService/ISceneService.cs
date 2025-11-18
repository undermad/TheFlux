using System;
using Cysharp.Threading.Tasks;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    public interface ISceneService
    {
        UniTask LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes);
    }
}