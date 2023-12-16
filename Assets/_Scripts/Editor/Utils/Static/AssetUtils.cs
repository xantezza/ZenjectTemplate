using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Editor.Utils.Static
{
    public static class AssetUtils
    {
        [MenuItem("Assets/To Entry Point", false, 10)]
        private static void ToEntryPoint()
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
        }

        [MenuItem("Assets/Select Project Context", false, 10)]
        private static void SelectProjectContext()
        {
            Selection.activeObject = Resources.Load<ProjectContext>("ProjectContext");
        }

        public static T GetAssetOfType<T>() where T : Object
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    return AssetDatabase.LoadAssetAtPath<T>(path);
                })
                .ToArray();

            if (assets.Length == 0)
            {
                Debug.LogWarning($"{typeof(T).Name} not found.");
                return default;
            }

            if (assets.Length > 1) Debug.LogWarning($"More than 1 {typeof(T).Name} founded. Get first found!");

            return assets.First();
        }
    }
}