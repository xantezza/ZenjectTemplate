using Infrastructure.Providers;
using Infrastructure.Services.Logging;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class GameStateSwitchButton : MonoBehaviour
    {
        private enum TargetStates
        {
            Menu = 2,
            Gameplay = 3
        }

        [SerializeField] private TargetStates _targetState = 0;
        [HideInInspector] [SerializeField] private Button _button;

        private IConditionalLoggingService _conditionalLoggingService;
        private GameLoopStateMachineProvider _gameLoopStateMachineProvider;

        [Inject]
        private void Inject(GameLoopStateMachineProvider gameLoopStateMachineProvider, IConditionalLoggingService conditionalLoggingService)
        {
            _gameLoopStateMachineProvider = gameLoopStateMachineProvider;
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

        private void OnClick()
        {
            switch (_targetState)
            {
                case TargetStates.Menu:
                    _gameLoopStateMachineProvider.StateMachine(this).Enter<LoadSceneState, SceneNames>(SceneNames.Menu);
                    break;
                case TargetStates.Gameplay:
                    _gameLoopStateMachineProvider.StateMachine(this).Enter<LoadSceneState, SceneNames>(SceneNames.Gameplay);
                    break;
                default:
                    _conditionalLoggingService.LogError($"Missing logic in {this}", LogTag.UI);
                    break;
            }
        }
    }
}