using UnityEngine;

namespace MetaBuddy.Util
{
    public static class LogExtensions
    {
        public static void LogWithoutStackTrace(LogType logType, string formattedMessage)
        {
            LogFormat(logType, formattedMessage + "\n");
        }

        private static void LogFormat(LogType logType, string format)
        {
            switch (logType)
            {
                case LogType.Error:
                    {
                        Debug.LogErrorFormat(format);
                        break;
                    }

                case LogType.Assert:
                    {
                        Debug.LogAssertionFormat(format);
                        break;
                    }

                case LogType.Warning:
                    {
                        Debug.LogWarningFormat(format);
                        break;
                    }

                case LogType.Log:
                    {
                        Debug.LogFormat(format);
                        break;
                    }

                default:
                    {
                        Debug.LogFormat(format);
                        break;
                    }
            }

        }
    }
}
