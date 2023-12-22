using Editor.Utils.Static;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.EditorWindows.Windows
{
    public class SceneSelector : EditorWindow
    {
        private string[] _scenes = new string[1];

        private void OnValidate()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;

            if (sceneCount == _scenes.Length) UpdateScenes(this);
        }

        [MenuItem("Window/Scene Selector", false, 10)]
        public static void OpenWindow()
        {
            var window = GetWindow<SceneSelector>();
            UpdateScenes(window);
            window.Show();
        }

        private void CreateGUI()
        {
            UpdateScenes(this);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Update list of scenes"))
            {
                UpdateScenes(this);
            }

            GUILayout.Space(50f);

            foreach (var sceneName in _scenes)
            {
                if (GUILayout.Button(sceneName))
                {
                    AssetDatabase.SaveAssets();
                    EditorSceneManager.OpenScene(sceneName);
                }
            }
        }

        private static void UpdateScenes(SceneSelector sceneSelector)
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenes = new string[sceneCount];
            for (var i = 0; i < sceneCount; i++) scenes[i] = SceneUtility.GetScenePathByBuildIndex(i);
            sceneSelector._scenes = scenes;
        }
    }
}