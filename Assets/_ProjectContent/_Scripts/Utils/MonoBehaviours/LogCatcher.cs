using System.Collections.Generic;
using Configs;
using IngameDebugConsole;
using UnityEngine;

namespace Utils.MonoBehaviours
{
    public class LogCatcher : MonoBehaviour
    {
        private static bool IsAwakened;

        private List<(string, string, LogType)> _logs;

        private void Awake()
        {
#if DEV

            if (IsAwakened)
            {
                Destroy(this);
                return;
            }

            if (transform.parent == null) DontDestroyOnLoad(this);

            IsAwakened = true;
            _logs = new List<(string, string, LogType)>(30);

            Application.logMessageReceived += CacheLogOnMessageReceived;
            DebugLogManager.OnInit += InGameDebugWindowInitialize;
#endif
        }

        private void InGameDebugWindowInitialize()
        {
            DebugLogManager.Instance.gameObject.SetActive(true);

            Application.logMessageReceived -= CacheLogOnMessageReceived;

            var logsCount = _logs.Count;
            for (var i = 0; i < logsCount; i++)
            {
                var log = _logs[i];
                SendLogToInGameDebugWindow(log.Item1, log.Item2, log.Item3);
            }

            _logs.Clear();
        }

        private void CacheLogOnMessageReceived(string condition, string stacktrace, LogType type)
        {
            _logs.Add((condition, stacktrace, type));
        }

        private void SendLogToInGameDebugWindow(string condition, string stacktrace, LogType type)
        {
            DebugLogManager.Instance.ReceivedLog(condition, stacktrace, type);
        }
    }
}