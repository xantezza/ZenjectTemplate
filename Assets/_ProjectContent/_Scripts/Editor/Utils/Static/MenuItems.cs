using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor.Utils.Static
{
    public static class MenuItems
    {
        [MenuItem("Assets/Project/Select Project Context", false, 6)]
        private static void SelectProjectContext()
        {
            Selection.activeObject = Resources.Load<ProjectContext>("ProjectContext");
        }

        [MenuItem("Assets/Project/Select Config Utility", false, 8)]
        private static void SelectConfigUtility()
        {
            Selection.activeObject = Resources.Load<ConfigUtility>("ConfigUtility");
        }

        [MenuItem("Assets/Project/Open PersistentDataPath", false, 10)]
        private static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}