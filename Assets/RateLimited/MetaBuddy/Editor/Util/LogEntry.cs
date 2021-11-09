using MetaBuddyLib.Log;

namespace MetaBuddy.Util
{
    public class LogEntry
    {
        public LogLevelFlags Flags { get; private set; }
        public string Message { get; private set; }

        public LogEntry(LogLevelFlags flags, string message)
        {
            Flags = flags;
            Message = message;
        }
    };
}
