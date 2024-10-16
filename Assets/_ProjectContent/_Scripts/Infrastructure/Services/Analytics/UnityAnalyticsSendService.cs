using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Services.Logging;
using Unity.Services.Analytics;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class UnityAnalyticsSendService : IAnalyticsSendService, IDisposable
    {
        private readonly ConditionalLoggingService _conditionalLoggingService;
        private bool _initialized;

        [Inject]
        public UnityAnalyticsSendService(ConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            Application.focusChanged += OnApplicationFocus;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnApplicationFocus;
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus) return;
            if (!Application.isPlaying) return;

            AnalyticsService.Instance.Flush();
        }

        public void SendEvent(string eventName)
        {
            Initialize();

            _conditionalLoggingService.Log($"{eventName} sent", LogTag.Analytics);

            AnalyticsService.Instance.RecordEvent(eventName);
        }

        public void SendEvent(string eventName, Dictionary<string, object> paramsDictionary)
        {
            Initialize();

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