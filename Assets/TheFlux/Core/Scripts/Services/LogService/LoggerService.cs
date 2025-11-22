using System.Collections.Generic;

using UnityEngine;

using VContainer;

namespace TheFlux.Core.Scripts.Services.LogService
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical,
        None
    }

    public class LoggerService
    {
        public LogLevel GlobalLevel = LogLevel.Debug;
        public LoggerColorMode ColorMode = LoggerColorMode.CategoryOnly;
        private readonly Dictionary<LogCategory, (bool active, Color color)> loggerCategories = new();

        private readonly LoggerConfig _loggerConfig;

        [Inject]
        public LoggerService(LoggerConfig loggerConfig)
        {
            _loggerConfig = loggerConfig;
            ApplyConfig();
            Log("Logger config loaded.");
        }

        private void ApplyConfig()
        {
            GlobalLevel = _loggerConfig.globalLevel;
            ColorMode = _loggerConfig.colorMode;

            var categories = _loggerConfig.categories;

            foreach (var entry in categories)
            {
                var color = entry.color;
                loggerCategories[entry.logCategory] = (entry.active, color);
            }
        }

        public void Log(string message, LogLevel level = LogLevel.Info, LogCategory category = LogCategory.General)
        {
            if (level < GlobalLevel) return;
            if (!loggerCategories.ContainsKey(category) || !loggerCategories[category].active) return;
            var categoryColor = loggerCategories[category].color;
            var categoryColorHex = ColorUtility.ToHtmlStringRGB(categoryColor);
            var fullMessage = ColorMode switch
            {
                LoggerColorMode.NoColor => $"[{level}] [{category}] {message}",
                LoggerColorMode.CategoryOnly => $"[{level}] <color=#{categoryColorHex}>[{category}]</color> {message}",
                LoggerColorMode.FullMessage => $"<color=#{categoryColorHex}>[{level}] [{category}] {message}</color>",
                _ => ""
            };

            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(fullMessage);
                    break;

                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError(fullMessage);
                    break;

                case LogLevel.Debug:
                case LogLevel.Info:
                case LogLevel.None:
                default:
                    Debug.Log(fullMessage);
                    break;
            }
        }
    }
}