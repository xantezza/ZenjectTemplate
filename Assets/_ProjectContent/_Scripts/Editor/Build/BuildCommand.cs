using System;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;
using UnityEngine;

namespace Editor
{
    public class BuildCommand
    {
        
#if DEV
        [MenuItem("Build/DEV_Windows/Confirm")]
        public static void BuildWindows()
        {
            BuildGame(BuildTarget.StandaloneWindows64, "Windows", true);
        }

        [MenuItem("Build/DEV_WebGL/Confirm")]
        public static void BuildWebGL()
        {
            BuildGame(BuildTarget.WebGL, "WebGL", true);
        }

        [MenuItem("DEV/To Production/Confirm")]
        public static void DEVToProd()
        {
            var currentGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, currentDefines.Replace(";DEV", "").Replace("DEV", ""));
            CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.None);
        }
#else
        [MenuItem("Build/PROD_Windows/Confirm")]
        public static void BuildWindows()
        {
            BuildGame(BuildTarget.StandaloneWindows64, "Windows", false);
        }

        [MenuItem("Build/PROD_WebGL/Confirm")]
        public static void BuildWebGL()
        {
            BuildGame(BuildTarget.WebGL, "WebGL", false);
        }
        
        [MenuItem("PROD/To DEV/Confirm")]
        public static void ProdToDEV()
        {
            var currentGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, currentDefines + ";DEV");
            CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.None);
        }

#endif
        private static void BuildGame(BuildTarget target, string platformFolderName, bool isDev)
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
                    PlayerSettings.WebGL.exceptionSupport = isDev ? WebGLExceptionSupport.FullWithoutStacktrace : WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
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

            if (isDev)
            {
                options.options
                    |= BuildOptions.Development
                       | BuildOptions.ConnectWithProfiler
                       | BuildOptions.EnableDeepProfilingSupport
                       | BuildOptions.AllowDebugging;
            }

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