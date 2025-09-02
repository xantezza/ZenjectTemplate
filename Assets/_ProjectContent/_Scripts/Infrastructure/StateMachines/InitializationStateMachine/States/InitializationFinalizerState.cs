using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using JetBrains.Annotations;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private readonly IAnalyticsService _analyticsService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;
        private readonly ISceneLoaderService _sceneLoaderService;

        protected InitializationFinalizerState(
            InitializationStateMachine stateMachine,
            ISceneLoaderService sceneLoaderService,
            IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            IAnalyticsService analyticsService,
            IAssetReferenceProvider assetReferenceProvider
        ) : base(stateMachine)
        {
            _sceneLoaderService = sceneLoaderService;
            _analyticsService = analyticsService;
            _assetReferenceProvider = assetReferenceProvider;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _analyticsService.SendEvent("LOAD_FINISHED", new Dictionary<string, object> {["LOAD_TIME"] = Time.realtimeSinceStartup});
            CollectHardwareData();
            await _sceneLoaderService.LoadScene(_assetReferenceProvider.MenuScene, OnSceneLoaded);
        }

        private async void OnSceneLoaded()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<MenuState>();
        }

        private void CollectHardwareData()
        {
            var sb = new System.Text.StringBuilder();

            sb.Append("\n")
                .Append("CPU: ")
                .Append(SystemInfo.processorType)
                .Append(" [")
                .Append(SystemInfo.processorCount)
                .Append(" cores]\n");

            sb.Append("RAM: ")
                .Append(SystemInfo.systemMemorySize)
                .Append(" MB\n");

            sb.Append("Graphics API: ")
                .Append(SystemInfo.graphicsDeviceVersion)
                .Append("\n");

            sb.Append("GPU: ")
                .Append(SystemInfo.graphicsDeviceName)
                .Append("\n");

            sb.Append("VRAM: ")
                .Append(SystemInfo.graphicsMemorySize)
                .Append("MB. Max texture size: ")
                .Append(SystemInfo.maxTextureSize)
                .Append("px. Shader level: ")
                .Append(SystemInfo.graphicsShaderLevel)
                .Append("\n");

            Resolution res = Screen.currentResolution;
            sb.Append("Screen: ")
                .Append(res.width)
                .Append('x')
                .Append(res.height)
#if UNITY_2022_2_OR_NEWER
                .Append("@")
                .Append(((int) Screen.currentResolution.refreshRateRatio.value).ToStringNonAlloc())
#else
                .Append("@")
                .Append(res.refreshRate)
#endif
                .Append("Hz\n");

            sb.Append("OS: ")
                .Append(SystemInfo.operatingSystem)
                .Append(" [")
                .Append(SystemInfo.deviceType)
                .Append(']')
                .Append('\n');

            var hardwareDataString = sb.ToString();
            _analyticsService.SendEvent("HARDWARE_DATA", new Dictionary<string, object> {["data"] = hardwareDataString});
        }
    }
}