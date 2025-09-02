using System.Collections.Generic;
using System.Text;
using IngameDebugConsole;
using UnityEngine;

namespace RuntimeDebug
{
    public class LogCatcher : MonoBehaviour
    {
#if DEV
        private static bool IsAwakened;

        private static List<(string, string, LogType)> _logs;

        private void OnEnable()
        {
            if (IsAwakened)
            {
                Destroy(this);
                return;
            }
            
            _logs.Clear();

            if (transform.parent == null) DontDestroyOnLoad(this);

            IsAwakened = true;
            _logs = new List<(string, string, LogType)>(60);

            Application.logMessageReceived += CacheLogOnMessageReceived;
            DebugLogManager.OnInit += InGameDebugWindowInitialize;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= CacheLogOnMessageReceived;
            DebugLogManager.OnInit -= InGameDebugWindowInitialize;
        }

        public static void CopyLogsToSystemBuffer()
        {
            var sb = new StringBuilder();

            foreach (var (item1, item2, logType) in _logs)
            {
                sb.AppendLine($"[{logType}]{item1}\n{item2}");
            }

            GUIUtility.systemCopyBuffer = sb.ToString();
        }

        private void InGameDebugWindowInitialize()
        {
            DebugLogManager.Instance.gameObject.SetActive(true);
            
            var logsCount = _logs.Count;
            for (var i = 0; i < logsCount; i++)
            {
                var log = _logs[i];
                SendLogToInGameDebugWindow(log.Item1, log.Item2, log.Item3);
            }
        }

        private void CacheLogOnMessageReceived(string condition, string stacktrace, LogType type)
        {
            _logs.Add((condition, stacktrace, type));
        }

        private void SendLogToInGameDebugWindow(string condition, string stacktrace, LogType type)
        {
            DebugLogManager.Instance.ReceivedLog(condition, stacktrace, type);
        }
#endif
    }
}