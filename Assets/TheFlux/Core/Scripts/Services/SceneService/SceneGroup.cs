using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Services.SceneService
{
    [Serializable]
    public class SceneGroup
    {
        public SceneGroupsName groupName;
        public List<SceneData> scenes;

        public string FindSceneNameByType(SceneType sceneType)
        {
            return scenes.Find(scene => scene.sceneType == sceneType).Name;
        }
    }
}