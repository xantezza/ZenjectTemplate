using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
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

        private ConditionalLoggingService _conditionalLoggingService;
        private GameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private ISaveService _saveService;

        [Inject]
        private void Inject(GameLoopStateMachineFactory gameLoopStateMachineFactory, ISaveService saveService, ConditionalLoggingService conditionalLoggingService)
        {
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
                    _saveService.StoreSaveFile();
                    await _gameLoopStateMachineFactory.GetFrom(this).Enter<LoadingScreenState, SceneNames>(SceneNames.Menu);
                    break;
                case TargetStates.Gameplay:
                    await _gameLoopStateMachineFactory.GetFrom(this).Enter<LoadingScreenState, SceneNames>(SceneNames.Gameplay);
                    break;
                default:
                    _conditionalLoggingService.LogError($"Missing logic in {this}", LogTag.UI);
                    break;
            }
        }
    }
}