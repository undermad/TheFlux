namespace TheFlux.Core.Scripts.Services.LogService
{
    public static class LogService
    {
        private static Logger _logger;

        internal static void InjectLogger(Logger logger)
        {
            _logger = logger;
        }

        public static void Log(
            string message,
            LogLevel level = LogLevel.Info,
            LogCategory category = LogCategory.General)
        {
            _logger.Log(message, level, category);
        }
    }
}