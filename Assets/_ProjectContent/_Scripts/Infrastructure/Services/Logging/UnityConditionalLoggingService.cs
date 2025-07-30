using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.Logging
{
    [UsedImplicitly]
    public class UnityConditionalLoggingService : IConditionalLoggingService
    {
        private readonly Dictionary<LogTag, Color> _tagColors = new()
        {
            { LogTag.InitializationStateMachine, new Color(0.3f, 1, 0) },
            { LogTag.GameLoopStateMachine, Color.yellow },
        };

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
    }
}