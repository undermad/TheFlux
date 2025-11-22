using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.Serialization;

namespace TheFlux.Core.Scripts.Services.LogService
{
    public enum LoggerColorMode
    {
        NoColor,
        CategoryOnly,
        FullMessage
    }

    public enum LogCategory
    {
        General,
        UI,
    }

    [Serializable]
    public class CategoryEntry
    {
        [FormerlySerializedAs("name")] public LogCategory logCategory;
        public bool active = true;
        public Color color = Color.white;
    }

    [CreateAssetMenu(fileName = "LoggerConfig", menuName = "Logging/LoggerConfig")]
    public class LoggerConfig : ScriptableObject
    {
        public bool groupByPrefix = false;
        public LoggerColorMode colorMode = LoggerColorMode.CategoryOnly;
        public LogLevel globalLevel = LogLevel.Debug;
        public List<CategoryEntry> categories = new List<CategoryEntry>();

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (categories != null && categories.Count != 0) return;
            categories = new List<CategoryEntry>();
            foreach (LogCategory category in Enum.GetValues(typeof(LogCategory)))
            {
                categories.Add(new CategoryEntry
                {
                    logCategory = category,
                    active = true,
                    color = Color.white
                });
            }
            EditorUtility.SetDirty(this);
#endif
        }
    }
}