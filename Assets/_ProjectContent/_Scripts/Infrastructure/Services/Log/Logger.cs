using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Infrastructure.Services.Analytics;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Infrastructure.Services.Log
{
    public abstract class Logger
    {
        public static event Action<string> OnError;
        
        private static IAnalyticsService _analyticsService;

#if DEV
        private static readonly Dictionary<LogTag, Color> _tagColors = new()
        {
            {LogTag.InitializationStateMachine, new Color(0.3f, 1, 0)},
            {LogTag.GameLoopStateMachine, Color.yellow},
            {LogTag.Analytics, Color.cyan}
        };
#endif

        [Inject]
        private void Inject(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
            Application.logMessageReceived += OnApplicationLogMessageReceived;
        }

        ~Logger()
        {
            Application.logMessageReceived -= OnApplicationLogMessageReceived;
        }
        
        private static readonly LogTag[] _tagsToExclude = { };
        private static readonly LogTag[] _warningTagsToExclude = { };

        [Conditional("DEV")]
        public static void Log(string text, LogTag tag = LogTag.Default)
        {
            if (_tagsToExclude.Contains(tag)) return;
            InternalLog(text, tag);
        }

        [Conditional("DEV")]
        public static void Warn(string text, LogTag tag = LogTag.Default)
        {
            if (_warningTagsToExclude.Contains(tag)) return;
            InternalLogWarning(text, tag);
        }

        public static void Error(string text, LogTag tag = LogTag.Default)
        {
            InternalLogError(text, tag);
            OnError?.Invoke(text);
        }

        public static void CritError(string text, LogTag tag = LogTag.CritError)
        {
            InternalLogCritError(text, tag);
            OnError?.Invoke(text);
        }

        private static void InternalLog(string text, LogTag tag)
        {
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}]</color> {text}");
            }
            else
            {
                Debug.LogFormat("[{0}] {1}", tag, text);
            }
        }

        private static void InternalLogWarning(string text, LogTag tag)
        {
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}]</color> {text}");
            }
            else
            {
                Debug.LogWarningFormat("[{0}] {1}", tag, text);
            }
        }

        private static void InternalLogError(string text, LogTag tag)
        {
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}]</color> {text}");
            }
            else
            {
                Debug.LogErrorFormat("[{0}] {1}", tag, text);
            }
        }

        private static void InternalLogCritError(string text, LogTag tag)
        {
            Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}>[{tag}]</color> {text}");
        }
        
        private void OnApplicationLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type is LogType.Exception or LogType.Error)
            {
                SendErrorEvent($"{condition}, \n {stacktrace}");
            }
        }
        
        private void SendErrorEvent(string text)
        {
            _analyticsService.SendEvent("CRITICAL_ERROR", new Dictionary<string, object>() {["ERROR_NAME"] = text});
        }
    }
}