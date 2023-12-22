using IngameDebugConsole;
using Tayx.Graphy;

namespace RuntimeDebug.ConsoleMethods
{
    public class Tools
    {
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
    }
}