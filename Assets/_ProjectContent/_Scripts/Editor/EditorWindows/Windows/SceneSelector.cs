using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.EditorWindows.Windows
{
    public class SceneSelector : EditorWindow
    {
        private string[] _scenes = new string[1];

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
            if (GUILayout.Button("Update list of scenes", new GUIStyle(GUI.skin.button) {alignment = TextAnchor.MiddleLeft}))
            {
                UpdateScenes(this);
            }

            GUILayout.Space(25f);

            for (var index = 0; index < _scenes.Length; index++)
            {
                var sceneName = _scenes[index];
                if (GUILayout.Button($"{index + 1}: {Path.GetFileName(sceneName).Replace(".unity", "")}", new GUIStyle(GUI.skin.button) {alignment = TextAnchor.MiddleLeft}))
                {
                    AssetDatabase.SaveAssets();
                    if (SceneManager.GetActiveScene().buildIndex != -1) EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
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