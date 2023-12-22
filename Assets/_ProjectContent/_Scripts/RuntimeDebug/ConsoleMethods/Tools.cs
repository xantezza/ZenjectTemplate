using IngameDebugConsole;
using Tayx.Graphy;

namespace RuntimeDebug.ConsoleMethods
{
    public static class Tools
    {
        [ConsoleMethod("graphy.toggle-active", "toggles graphy state"), UnityEngine.Scripting.Preserve]
        public static void ToggleGraphyActive()
        {
            GraphyManager.Instance.ToggleActive();
        }

        [ConsoleMethod("graphy.toggle-mode", "toggles graphy view mode"), UnityEngine.Scripting.Preserve]
        public static void ToggleGraphyMode()
        {
            GraphyManager.Instance.ToggleModes();
        }
    }
}