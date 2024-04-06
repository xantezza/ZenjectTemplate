using System;
using System.Collections;
using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class LoadingScreenState : IState, IPayloadedState<Action>, IPayloadedState<SceneNames>, IPayloadedState<SceneNames, Action>
    {
        private const int LoadingSceneNumber = 1;
        public static event Action<float> OnLoadSceneProgressUpdated;

        private readonly ICoroutineRunnerService _coroutineRunnerService;
        private readonly GameLoopStateMachine _stateMachine;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IConditionalLoggingService _conditionalLoggingService;
        private InfrastructureConfig _infrastructureConfig;

        private SceneNames _cachedSceneToLoadAfterLoadingSceneLoad;
        private Action _cachedCallback;

        [Inject]
        public LoadingScreenState(
            GameLoopStateMachine stateMachine,
            ISceneLoaderService sceneLoaderService,
            ICoroutineRunnerService coroutineRunnerService,
            IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _stateMachine = stateMachine;
            _sceneLoaderService = sceneLoaderService;
            _coroutineRunnerService = coroutineRunnerService;
            Remote.OnInitializeAny += OnRemoteInitializeAny;
        }

        private void OnRemoteInitializeAny()
        {
            _infrastructureConfig = Remote.InfrastructureConfig;
        }

        private async UniTask ToNextState()
        {
            switch (_cachedSceneToLoadAfterLoadingSceneLoad)
            {
                case SceneNames.Menu:
                    await _stateMachine.Enter<MenuState>();
                    break;
                case SceneNames.Gameplay:
                    await _stateMachine.Enter<GameplayState>();
                    break;
                default:
                    _conditionalLoggingService.LogError("Missing logic", LogTag.GameLoopStateMachine);
                    break;
            }
        }

        public UniTask Enter(Action onLoadingSceneLoad)
        {
            _sceneLoaderService.LoadScene(LoadingSceneNumber, onLoadingSceneLoad);
            return default;
        }

        public UniTask Enter(SceneNames sceneToLoadAfterLoadingSceneLoad)
        {
            _cachedSceneToLoadAfterLoadingSceneLoad = sceneToLoadAfterLoadingSceneLoad;
            _cachedCallback = null;
            _sceneLoaderService.LoadScene(LoadingSceneNumber, OnLoadingSceneLoaded);
            return default;
        }

        public UniTask Enter(SceneNames payload, Action onPayloadSceneLoad)
        {
            _cachedSceneToLoadAfterLoadingSceneLoad = payload;
            _cachedCallback = onPayloadSceneLoad;
            _sceneLoaderService.LoadScene(LoadingSceneNumber, OnLoadingSceneLoaded);
            return default;
        }

        private void OnLoadingSceneLoaded()
        {
            _coroutineRunnerService.StartCoroutine(FakeLoading());

            _sceneLoaderService.LoadScene(
                _cachedSceneToLoadAfterLoadingSceneLoad,
                _cachedCallback ?? DefaultCallbackOnPayloadSceneLoaded,
                _infrastructureConfig.FakeTimeBeforeLoad
                + _infrastructureConfig.FakeMinimalLoadTime
                + _infrastructureConfig.FakeTimeAfterLoad
            );
        }

        private async void DefaultCallbackOnPayloadSceneLoaded()
        {
            await ToNextState();
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

        public UniTask Exit()
        {
            return default;
        }
    }
}