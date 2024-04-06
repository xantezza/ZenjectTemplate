using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScene
{
    public class LoadingProgressbar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private void OnEnable()
        {
            LoadingScreenState.OnLoadSceneProgressUpdated += OnLoadingStateSceneProgressUpdated;
        }

        private void OnDisable()
        {
            LoadingScreenState.OnLoadSceneProgressUpdated -= OnLoadingStateSceneProgressUpdated;
        }

        private void OnLoadingStateSceneProgressUpdated(float progress01)
        {
            _slider.value = progress01;
        }
    }
}