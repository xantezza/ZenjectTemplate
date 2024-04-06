using System;
using System.Collections.Generic;
using System.Text;

namespace UniRx.Diagnostics
{
    public static class LogEntryExtensions
    {
        public static IDisposable LogToUnityDebug(this IObservable<LogEntry> source)
        {
            return source.Subscribe(new UnityDebugSink());
        }
    }
}