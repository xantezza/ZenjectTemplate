using UnityEngine;

namespace Infrastructure.Services.Logging
{
    public class UnityConditionalLoggingService : IConditionalLoggingService
    {
        protected override void InternalLog(string text, LogTag tag)
        {
            Debug.LogFormat("[{0}] {1}", tag, text);
        }

        protected override void InternalLogWarning(string text, LogTag tag)
        {
            Debug.LogWarningFormat("[{0}] {1}", tag, text);
        }

        protected override void InternalLogError(string text, LogTag tag)
        {
            Debug.LogErrorFormat("[{0}] {1}", tag, text);
        }
    }
}