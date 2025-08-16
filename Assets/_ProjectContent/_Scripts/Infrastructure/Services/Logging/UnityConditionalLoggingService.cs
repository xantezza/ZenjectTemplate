using System.Collections.Generic;
using Infrastructure.Services.Analytics;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Logging
{
    [UsedImplicitly]
    public class UnityConditionalLoggingService : ConditionalLoggingService
    {
        private IAnalyticsService _analyticsService;

        [Inject]
        private void Inject(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        protected override void InternalLog(string text, LogTag tag)
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

        protected override void InternalLogWarning(string text, LogTag tag)
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

        protected override void InternalLogError(string text, LogTag tag)
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

        protected override void InternalLogCritError(string text, LogTag tag)
        {
            Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}>[{tag}]</color> {text}");
            _analyticsService.SendEvent("CRITICAL_ERROR", new Dictionary<string, object>(){["ERROR_NAME"] = text});
        }
    }
}