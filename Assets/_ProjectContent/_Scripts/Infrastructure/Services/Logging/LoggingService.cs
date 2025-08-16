using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Services.Logging
{
    // Must be abstract for Conditional attribute to work
    public abstract class LoggingService
    {
#if DEV
        protected readonly Dictionary<LogTag, Color> _tagColors = new()
        {
            {LogTag.InitializationStateMachine, new Color(0.3f, 1, 0)},
            {LogTag.GameLoopStateMachine, Color.yellow},
            {LogTag.Analytics, Color.cyan}
        };

        private readonly LogTag[] _tagsToExclude = { };
        private readonly LogTag[] _warningTagsToExclude = { };
        private readonly LogTag[] _errorTagsToExclude = { };
#endif

        [Conditional("DEV")]
        public void Log(string text, LogTag tag = LogTag.Default)
        {
#if DEV
            if (_tagsToExclude.Contains(tag)) return;
#endif
            InternalLog(text, tag);
        }

        [Conditional("DEV")]
        public void LogWarning(string text, LogTag tag = LogTag.Default)
        {
#if DEV
            if (_warningTagsToExclude.Contains(tag)) return;
#endif
            InternalLogWarning(text, tag);
        }

        [Conditional("DEV")]
        public void LogError(string text, LogTag tag = LogTag.Default)
        {
#if DEV
            if (_errorTagsToExclude.Contains(tag)) return;
#endif
            InternalLogError(text, tag);
        }

        public void LogCritError(string text, LogTag tag = LogTag.CritError)
        {
            InternalLogCritError(text, tag);
        }

        protected abstract void InternalLog(string text, LogTag tag);

        protected abstract void InternalLogWarning(string text, LogTag tag);

        protected abstract void InternalLogError(string text, LogTag tag);

        protected abstract void InternalLogCritError(string text, LogTag tag);
    }
}