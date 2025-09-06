using IngameDebugConsole;
using NUnit.Framework;
using Tayx.Graphy;

namespace RuntimeDebug.ConsoleMethods
{
    public class Tools
    {
#if DEV
        [ConsoleMethod("copy-logs", "puts all logs to system buffer"), UnityEngine.Scripting.Preserve]
        public static void CopyLogsToSystemBuffer()
        {
            LogCatcher.CopyLogsToSystemBuffer();
        }
        
        [ConsoleMethod("graphy.toggle-active", "toggle active"), UnityEngine.Scripting.Preserve]
        public static void GraphyToggleActive()
        {
            GraphyManager.Instance.ToggleActive();
        }

        [ConsoleMethod("graphy.toggle-mode", "toggle mode"), UnityEngine.Scripting.Preserve]
        public static void GraphyToggleMode()
        {
            GraphyManager.Instance.ToggleModes();
        }
        
#endif
    }
}