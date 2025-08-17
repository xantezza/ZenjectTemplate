using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Logging;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Global
{
    [RequireComponent(typeof(Button))]
    public class GameLoopStateSwitchButton : MonoBehaviour
    {
        private enum TargetStates
        {
            Menu = 2,
            Gameplay = 3
        }

        [SerializeField] private TargetStates _targetState = 0;
        [HideInInspector] [SerializeField] private Button _button;

        private LoggingService _loggingService;
        private IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private IAssetReferenceProvider _assetReferenceProvider;
        private ISceneLoaderService _sceneLoaderService;

        [Inject]
        private void Inject(
            IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            ISceneLoaderService sceneLoaderService,
            LoggingService loggingService,
            IAssetReferenceProvider assetReferenceProvider)
        {
            _sceneLoaderService = sceneLoaderService;
            _assetReferenceProvider = assetReferenceProvider;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
            _loggingService = loggingService;
        }

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private async void OnClick()
        {
            switch (_targetState)
            {
                case TargetStates.Menu:
                    await _sceneLoaderService.LoadScene(_assetReferenceProvider.MenuScene, OnMenuSceneLoaded);
                    break;
                case TargetStates.Gameplay:
                    await _sceneLoaderService.LoadScene(_assetReferenceProvider.GamePlayScene, OnGameplaySceneLoaded);
                    break;
                default:
                    _loggingService.LogCritError($"Missing logic in [{this}, {gameObject.name}]", LogTag.UI);
                    break;
            }
        }

        private async void OnMenuSceneLoaded()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<MenuState>();
        }

        private async void OnGameplaySceneLoaded()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<GameplayState>();
        }
    }
}