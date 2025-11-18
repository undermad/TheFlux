using System;
using System.Collections.Generic;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    [Serializable]
    public class SceneGroup
    {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public string FindSceneNameByType(SceneType sceneType)
        {
            return Scenes.Find(scene => scene.sceneType == sceneType).Name;
        }
    }
}