using System;
using Eflatun.SceneReference;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    [Serializable]
    public class SceneData
    {
        public SceneReference reference;
        public SceneType sceneType;
        public string Name => reference.Name;
    }
}