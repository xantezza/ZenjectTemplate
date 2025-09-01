using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Log;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logger = Infrastructure.Services.Log.Logger;

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

        private IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private IAssetReferenceProvider _assetReferenceProvider;
        private ISceneLoaderService _sceneLoaderService;

        [Inject]
        private void Inject(
            IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            ISceneLoaderService sceneLoaderService,
            IAssetReferenceProvider assetReferenceProvider)
        {
            _sceneLoaderService = sceneLoaderService;
            _assetReferenceProvider = assetReferenceProvider;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
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
                    Logger.CritError($"Missing logic in [{this}, {gameObject.name}]", LogTag.UI);
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