using BepInEx.Logging;

namespace Silkworm.Utils
{
    public static class LogUtils
    {
        private static ManualLogSource Log;

        static LogUtils()
        {
            Log = Logger.CreateLogSource("Silkworm");
        }

        public static void Init(ManualLogSource log)
        {
            Log = log;
        }

        public static bool IsLogLevel(LogLevel level)
        {
            return Logger.ListenedLogLevels.HasFlag(level);
        }

        public static void LogFatal(object data) => Log.LogFatal(data);

        public static void LogError(object data) => Log.LogError(data);

        public static void LogWarning(object data) => Log.LogWarning(data);

        public static void LogMessage(object data) => Log.LogMessage(data);

        public static void LogInfo(object data) => Log.LogInfo(data);

        public static void LogDebug(object data) => Log.LogDebug(data);

        public static void LogDebugFatal(object data)
        {
            if (IsLogLevel(LogLevel.Debug))
                LogFatal(data);
        }

        public static void LogDebugError(object data)
        {
            if (IsLogLevel(LogLevel.Debug))
                LogDebug(data);
        }

        public static void LogDebugWarning(object data)
        {
            if (IsLogLevel(LogLevel.Debug))
                LogWarning(data);
        }
    }
}
