using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace TheFlux.Core.Scripts.Services.LogService
{

    public class Logger
    {
        private LogLevel globalLevel = LogLevel.Debug;
        private LoggerColorMode colorMode = LoggerColorMode.CategoryOnly;
        private readonly Dictionary<LogCategory, (bool active, Color color)> loggerCategories = new();

        private readonly LoggerConfig loggerConfig;

        [Inject]
        public Logger(LoggerConfig loggerConfig)
        {
            this.loggerConfig = loggerConfig;
            ApplyConfig();
            LogService.InjectLogger(this);
            LogService.Log("Logger initialized");
        }

        private void ApplyConfig()
        {
            globalLevel = loggerConfig.globalLevel;
            colorMode = loggerConfig.colorMode;

            var categories = loggerConfig.categories;

            foreach (var entry in categories)
            {
                var color = entry.color;
                loggerCategories[entry.logCategory] = (entry.active, color);
            }
        }

        public void Log(string message, LogLevel level = LogLevel.Info, LogCategory category = LogCategory.General)
        {
            if (level < globalLevel) return;
            if (!loggerCategories.ContainsKey(category) || !loggerCategories[category].active) return;
            var categoryColor = loggerCategories[category].color;
            var categoryColorHex = ColorUtility.ToHtmlStringRGB(categoryColor);
            var fullMessage = colorMode switch
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