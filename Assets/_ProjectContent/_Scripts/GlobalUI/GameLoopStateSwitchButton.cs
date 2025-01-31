using System;
using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace GlobalUI
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

        private IConditionalLoggingService _conditionalLoggingService;
        private IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private ISaveService _saveService;
        private IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        private void Inject(IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            ISaveService saveService,
            IConditionalLoggingService conditionalLoggingService,
            IAssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
            _saveService = saveService;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
            _conditionalLoggingService = conditionalLoggingService;
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
                    await _gameLoopStateMachineFactory.GetFrom(this)
                        .Enter<LoadingScreenState, AssetReference, Action>(_assetReferenceProvider.MenuScene, OnLoadingScreenLoadedToMenu);
                    break;
                case TargetStates.Gameplay:
                    await _gameLoopStateMachineFactory.GetFrom(this)
                        .Enter<LoadingScreenState, AssetReference, Action>(_assetReferenceProvider.GamePlayScene, OnLoadingScreenLoadedToGameplay);
                    break;
                default:
                    _conditionalLoggingService.LogError($"Missing logic in [{this}, {gameObject.name}]", LogTag.UI);
                    break;
            }
        }

        private async void OnLoadingScreenLoadedToMenu()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<MenuState>();
        }

        private async void OnLoadingScreenLoadedToGameplay()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<GameplayState>();
        }
    }
}