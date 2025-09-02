using System;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEditor.Build.Reporting;
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

        [MenuItem("Build/Windows")]
        public static void BuildWindows()
        {
            BuildGame(BuildTarget.StandaloneWindows64, "Windows");
        }

        [MenuItem("Build/WebGL")]
        public static void BuildWebGL()
        {
            BuildGame(BuildTarget.WebGL, "WebGL");
        }

        private static void BuildGame(BuildTarget target, string platformFolderName)
        {
            var projectName = Path.GetFileName(Application.productName);
            var date = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");

            var platformFolderPath = Path.Combine(@"D:\UnityBuilds", platformFolderName);
            if (!Directory.Exists(platformFolderPath))
                Directory.CreateDirectory(platformFolderPath);

            var buildFolderName = $"{projectName}_{date}";
            var buildFolderPath = Path.Combine(platformFolderPath, buildFolderName);

            if (!Directory.Exists(buildFolderPath))
                Directory.CreateDirectory(buildFolderPath);

            string buildPath;

            switch (target)
            {
                case BuildTarget.WebGL:
                    buildPath = buildFolderPath;
                    break;
                case BuildTarget.StandaloneWindows64:
                    buildPath = Path.Combine(buildFolderPath, $"{projectName}.exe");
                    break;
                default:
                    throw new NotImplementedException();
            }

            var scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
                scenes[i] = EditorBuildSettings.scenes[i].path;

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = target,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize} bytes, saved to {buildPath}");

                if (target == BuildTarget.WebGL)
                {
                    var zipPath = buildFolderPath + ".zip";

                    if (File.Exists(zipPath))
                        File.Delete(zipPath);

                    ZipFile.CreateFromDirectory(buildFolderPath, zipPath);
                    Debug.Log($"WebGL build zipped to {zipPath}");

                    try
                    {
                        Directory.Delete(buildFolderPath, true);
                        Debug.Log($"Deleted original WebGL build folder: {buildFolderPath}");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to delete WebGL build folder: {e.Message}");
                    }
                }
            }
            else
            {
                Debug.LogError("Build failed");
            }
        }
    }
}