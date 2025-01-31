using System;
using System.Collections;
using Configs;
using Configs.RemoteConfig;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class LoadingScreenState : BaseGameLoopState, IPayloadedState<Action>, IPayloadedState<AssetReference, Action>
    {
        public static event Action<float> OnLoadSceneProgressUpdated;

        private readonly ICoroutineRunnerService _coroutineRunnerService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;
        private InfrastructureConfig _infrastructureConfig;

        private AssetReference _cachedSceneToLoadAfterLoadingSceneLoad;
        private Action _cachedCallback;

        [Inject]
        public LoadingScreenState(
            GameLoopStateMachine stateMachine,
            ISceneLoaderService sceneLoaderService,
            ICoroutineRunnerService coroutineRunnerService,
            IAssetReferenceProvider assetReferenceProvider
        ) : base(stateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
            _sceneLoaderService = sceneLoaderService;
            _coroutineRunnerService = coroutineRunnerService;
            RemoteConfig.OnInitializeAny += OnRemoteInitializeAny;
        }

        public UniTask Enter(Action onLoadingSceneLoad)
        {
            _sceneLoaderService.LoadScene(_assetReferenceProvider.LoadingScene, false, onLoadingSceneLoad);
            return default;
        }

        public UniTask Enter(AssetReference sceneToLoadAfterLoadingScreen, Action onPayloadSceneLoad)
        {
            _cachedSceneToLoadAfterLoadingSceneLoad = sceneToLoadAfterLoadingScreen;
            _cachedCallback = onPayloadSceneLoad;
            _sceneLoaderService.LoadScene(_assetReferenceProvider.LoadingScene, false, OnLoadingSceneLoaded);
            return default;
        }

        private void OnLoadingSceneLoaded()
        {
            _coroutineRunnerService.StartCoroutine(FakeLoading());

            _sceneLoaderService.LoadScene(
                _cachedSceneToLoadAfterLoadingSceneLoad,
                false,
                _cachedCallback,
                _infrastructureConfig.FakeTimeBeforeLoad
                + _infrastructureConfig.FakeMinimalLoadTime
                + _infrastructureConfig.FakeTimeAfterLoad
            );
        }


        private IEnumerator FakeLoading()
        {
            OnLoadSceneProgressUpdated?.Invoke(0);

            yield return new WaitForSeconds(_infrastructureConfig.FakeTimeBeforeLoad);

            yield return null;

            for (float timePassed = 0; timePassed < _infrastructureConfig.FakeMinimalLoadTime; timePassed += Time.unscaledDeltaTime)
            {
                OnLoadSceneProgressUpdated?.Invoke(Mathf.Clamp01(timePassed / _infrastructureConfig.FakeMinimalLoadTime));
                yield return null;
            }
        }

        private void OnRemoteInitializeAny()
        {
            _infrastructureConfig = RemoteConfig.InfrastructureConfig;
        }
    }
}