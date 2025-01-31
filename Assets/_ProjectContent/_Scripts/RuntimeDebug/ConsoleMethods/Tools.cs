using IngameDebugConsole;
using NUnit.Framework;
using Tayx.Graphy;

namespace RuntimeDebug.ConsoleMethods
{
    public class Tools
    {
#if DEV

        [ConsoleMethod("graphy.toggle-a", "toggle active"), UnityEngine.Scripting.Preserve]
        public static void GraphyToggleActive()
        {
            GraphyManager.Instance.ToggleActive();
        }

        [ConsoleMethod("graphy.toggle-m", "toggle mode"), UnityEngine.Scripting.Preserve]
        public static void GraphyToggleMode()
        {
            GraphyManager.Instance.ToggleModes();
        }
#endif
    }
}