using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Editor.Utils.Static
{
    public static class MenuItems
    {
        [MenuItem("Assets/Project/To Entry Point", false, 7)]
        private static void ToEntryPoint()
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
        }

        [MenuItem("Assets/Project/Select Project Context", false, 5)]
        private static void SelectProjectContext()
        {
            Selection.activeObject = Resources.Load<ProjectContext>("ProjectContext");
        }

        [MenuItem("Assets/Project/Select Config Utility", false, 8)]
        private static void SelectConfigUtility()
        {
            Selection.activeObject = Resources.Load<ConfigUtility>("ConfigUtility");
        }

        [MenuItem("Tools/Open PersistentDataPath", false, 8)]
        private static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}