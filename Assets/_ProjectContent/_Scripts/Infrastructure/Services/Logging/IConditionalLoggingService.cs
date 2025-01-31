using System.Diagnostics;

namespace Infrastructure.Services.Logging
{
    // Fake interface for consistency in injection methods and for Conditional attribute
    public abstract class IConditionalLoggingService
    {
        [Conditional("DEV")]
        public void Log(string text, LogTag tag = LogTag.Default)
        {
            InternalLog(text, tag);
        }

        [Conditional("DEV")]
        public void LogWarning(string text, LogTag tag = LogTag.Default)
        {
            InternalLogWarning(text, tag);
        }

        [Conditional("DEV")]
        public void LogError(string text, LogTag tag = LogTag.Default)
        {
            InternalLogError(text, tag);
        }

        protected abstract void InternalLog(string text, LogTag tag);

        protected abstract void InternalLogWarning(string text, LogTag tag);

        protected abstract void InternalLogError(string text, LogTag tag);
    }
}