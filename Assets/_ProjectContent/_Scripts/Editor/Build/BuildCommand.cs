using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Editor.Utils;
using Editor.Utils.Static;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;
using UnityEditor.WebGL;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.Build
{
    public class BuildCommand
    {
        private const int PORT = 50000;
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

        [MenuItem("Build/DEV_WebGL AutoRun/Confirm")]
        public static void BuildWebGLAutoRun()
        {
            BuildGame(BuildTarget.WebGL, "WebGL", true, true);
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
        
        private static void BuildGame(BuildTarget target, string platformFolderName, bool isDev, bool autoRun = false)
        {
            if (EditorUserBuildSettings.activeBuildTarget != target)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);
                AddressableAssetSettings.BuildPlayerContent();
            }
            
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Make sure Addressables are set up.");
                return;
            }

            VersionProvider versionProvider = AssetUtils.GetAssetOfType<VersionProvider>();
            versionProvider.Minor++;
            EditorUtility.SetDirty(versionProvider);
            AssetDatabase.SaveAssets();
            var buildType = isDev ? "DEV" : "Production";
            var versionWithDate = $"{buildType}_{versionProvider.Major}.{versionProvider.Minor}_{DateTime.Now:dd.MM.yy}";
            PlayerSettings.bundleVersion = versionWithDate;
            Debug.Log("Version: " + versionWithDate);
            
            var projectName = Path.GetFileName(Application.productName);

            var platformFolderPath = Path.Combine(@"D:\UnityBuilds", platformFolderName);
            if (!Directory.Exists(platformFolderPath))
                Directory.CreateDirectory(platformFolderPath);

            var buildFolderName = $"{projectName}\\{versionWithDate}_{projectName}";
            var buildFolderPath = Path.Combine(platformFolderPath, buildFolderName);

            if (!Directory.Exists(buildFolderPath))
                Directory.CreateDirectory(buildFolderPath);

            string buildPath;

            switch (target)
            {
                case BuildTarget.WebGL:
                    UserBuildSettings.codeOptimization =  isDev ? WasmCodeOptimization.BuildTimes : WasmCodeOptimization.DiskSize;
                    PlayerSettings.WebGL.compressionFormat =  isDev ? WebGLCompressionFormat.Disabled : WebGLCompressionFormat.Brotli;
                    PlayerSettings.WebGL.exceptionSupport = isDev ? WebGLExceptionSupport.FullWithStacktrace : WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
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
                        if (autoRun)
                        {
                            PythonServerLauncher.StartPythonHttpServer(buildFolderPath, PORT);
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = $"http://localhost:{PORT}/index.html",
                                UseShellExecute = true
                            });
                        }
                        else
                        {
                            Directory.Delete(buildFolderPath, true);
                            Debug.Log($"Deleted original WebGL build folder: {buildFolderPath}");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to delete WebGL build folder: {e.Message}");
                    }
                }
                
                Process.Start(platformFolderPath);
            }
            else
            {
                Debug.LogError("Build failed");
            }
        }
    }
}