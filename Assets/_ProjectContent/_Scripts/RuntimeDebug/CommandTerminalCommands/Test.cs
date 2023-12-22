using Infrastructure.Services.SceneLoading;
using IngameDebugConsole;
using Tayx.Graphy;
using UnityEngine;

public class Test
{
    [ConsoleMethod("test.1", "1"), UnityEngine.Scripting.Preserve]
    public static void Test1()
    {
        Debug.Log(1);
    }

    [ConsoleMethod("test.2", "2", "SceneNames"), UnityEngine.Scripting.Preserve]
    public static void Test2(SceneNames sceneNames)
    {
        Debug.Log(sceneNames);
    }

    [ConsoleMethod("graphy.toggle-active", "3"), UnityEngine.Scripting.Preserve]
    public static void Test3()
    {
        GraphyManager.Instance.ToggleActive();
    }

    [ConsoleMethod("graphy.toggle-mode", "4"), UnityEngine.Scripting.Preserve]
    public static void Test4()
    {
        GraphyManager.Instance.ToggleModes();
    }
}