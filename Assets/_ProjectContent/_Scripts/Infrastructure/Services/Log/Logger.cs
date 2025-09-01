using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Infrastructure.Services.Analytics;
using JetBrains.Annotations;
using UnityEngine;
using Utils.Extensions;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Infrastructure.Services.Log
{
    public class Logger
    {
        public static event Action<string> OnError;
        
        private static IAnalyticsService _analyticsService;

        private static readonly LogTag[] _tagsToExclude = { };
        
        private static readonly Dictionary<LogTag, Color> _tagColors = new()
        {
            {LogTag.InitializationStateMachine, new Color(0.3f, 1, 0)},
            {LogTag.GameLoopStateMachine, new Color(0.35f, 0.9f, 0.35f)},
            {LogTag.SaveService, new Color(1f, 0.5f, 1f)},
            {LogTag.SceneLoader, new Color(1f, 1f, 1f)},
            {LogTag.Analytics, new Color(0.0f, 1f, 1f)},
            {LogTag.UnityServices, new Color(0.0f, 0.8f, 1f)}
        };

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
        

        [Conditional("DEV")]
        public static void Log(string text, LogTag tag = LogTag.Default)
        {
            if (_tagsToExclude.Contains(tag)) return;
            
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.Log($"{color.ToRichHEX(tag)} {text}");
            }
            else
            {
                Debug.LogFormat("[{0}] {1}", tag, text);
            }
        }

        [Conditional("DEV")]
        public static void Warn(string text, LogTag tag = LogTag.Default)
        {
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.LogWarning($"{color.ToRichHEX(tag)} {text}");
            }
            else
            {
                Debug.LogWarningFormat("[{0}] {1}", tag, text);
            }
        }

        public static void Error(string text, LogTag tag = LogTag.Default)
        {
            if (_tagColors.TryGetValue(tag, out Color color))
            {
                Debug.LogError($"{color.ToRichHEX("EXCEPTION")}{color.ToRichHEX(tag)} {text}");
            }
            else
            {
                Debug.LogErrorFormat("[{0}] {1}", tag, text);
            }
        }

        
        private void OnApplicationLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type is LogType.Exception or LogType.Error)
            {
                SendAnalyticsErrorEvent($"{condition}, \n {stacktrace}");
                
                OnError?.Invoke(condition);
            }
        }
        
        private void SendAnalyticsErrorEvent(string text)
        {
            _analyticsService.SendEvent("exception", new Dictionary<string, object>() {["content"] = text});
        }
    }
}