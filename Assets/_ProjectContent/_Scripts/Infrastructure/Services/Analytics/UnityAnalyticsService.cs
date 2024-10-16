using System.Collections.Generic;
using System.Text;
using Infrastructure.Services.Logging;
using Unity.Services.Analytics;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class UnityAnalyticsService : IAnalyticsService
    {
        private readonly ConditionalLoggingService _conditionalLoggingService;
        private bool _initialized;

        [Inject]
        public UnityAnalyticsService(ConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            Application.focusChanged += OnApplicationFocus;
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus) return;
            if (!Application.isPlaying) return;

            AnalyticsService.Instance.Flush();
        }

        public void SendEvent(string eventName)
        {
            _conditionalLoggingService.Log($"{eventName} sent", LogTag.Analytics);

            AnalyticsService.Instance.RecordEvent(eventName);
        }

        public void SendEvent(string eventName, Dictionary<string, object> paramsDictionary)
        {

            var customEvent = new CustomEvent(eventName);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{eventName} sent");

            foreach (var (key, value) in paramsDictionary)
            {
                stringBuilder.AppendLine($"\nwith param: {key}: {value}");
                customEvent.Add(key, value);
            }

            _conditionalLoggingService.Log(stringBuilder.ToString(), LogTag.Analytics);
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
}