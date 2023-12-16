using System;
using System.Diagnostics;
using Zenject;

namespace Plugins.Zenject.Source.Util
{
    [DebuggerStepThrough]
    [NoReflectionBaking]
    public class ZenjectException : Exception
    {
        public ZenjectException(string message)
            : base(message)
        {
        }

        public ZenjectException(
            string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
