using System;
using System.Collections.Generic;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    [Serializable]
    public class SceneGroup
    {
        public string groupName = "New Scene Group";
        public List<SceneData> scenes;

        public string FindSceneNameByType(SceneType sceneType)
        {
            return scenes.Find(scene => scene.sceneType == sceneType).Name;
        }
    }
}