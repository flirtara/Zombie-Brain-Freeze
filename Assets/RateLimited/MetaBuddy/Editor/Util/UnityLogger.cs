using UnityEngine;
using MetaBuddyLib.Log;

namespace MetaBuddy.Util
{
    public class UnityLogger : IMetaBuddyLogger, ILogHistory
    {
        private LogLevelFlags _levels;       

        public LogEntry LastLogEntry { get; private set; }

        public UnityLogger()
        {
            _levels = LogLevelFlags.Critical | LogLevelFlags.Error | LogLevelFlags.Information;
        }

        private void SetFlag(bool active, LogLevelFlags flag)
        {
            _levels = active
                ? _levels | flag
                : _levels & (~flag);
        }

        private static string PrefixMessage(string message)
        {
            return $"metabuddy: {message}";
        }

        private static LogType ToLogType(LogLevelFlags logLevelFlags)
        {
            return logLevelFlags == LogLevelFlags.Error || logLevelFlags == LogLevelFlags.Critical
                ? LogType.Error
                : LogType.Log;
        }

        private void Log(LogLevelFlags logLevelFlags, string message, params object[] args)
        {
            var shouldLog = (logLevelFlags == LogLevelFlags.None) ||
                ((_levels & logLevelFlags) != LogLevelFlags.None);

            if(shouldLog)
            {
                var formattedMessage = string.Format(PrefixMessage(message), args);

                LastLogEntry = new LogEntry(logLevelFlags, message);

                LogExtensions.LogWithoutStackTrace
                (
                    ToLogType(logLevelFlags),
                    formattedMessage
                );
            }
        }

        public void SetInformation(bool active)
        {
            SetFlag(active, LogLevelFlags.Information);
        }

        public void LogInformation(string message, params object[] args)
        {
            Log(LogLevelFlags.Information, message, args);
        }

        public void SetError(bool active)
        {
            SetFlag(active, LogLevelFlags.Error);
        }

        public void LogError(string message, params object[] args)
        {
            Log(LogLevelFlags.Error, message, args);
        }

        public void LogNone(string message, params object[] args)
        {
            Log(LogLevelFlags.None, message, args);
        }

        public void SetDebug(bool active)
        {
            SetFlag(active, LogLevelFlags.Debug);
        }

        public void LogDebug(string message, params object[] args)
        {
            Log(LogLevelFlags.Debug, message, args);
        }

        public void SetCritical(bool active)
        {
            SetFlag(active, LogLevelFlags.Critical);
        }

        public void LogCritical(string message, params object[] args)
        {
            Log(LogLevelFlags.Critical, message, args);
        }
    }
}