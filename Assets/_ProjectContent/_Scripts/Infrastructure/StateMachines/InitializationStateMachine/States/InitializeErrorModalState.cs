using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Services.Modals;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializeErrorModalState : BaseInitializationState, IEnterableState, IDisposable
    {
        private readonly IModalPopupFactory _modalPopupFactory;

        protected InitializeErrorModalState(InitializationStateMachine stateMachine, IModalPopupFactory modalPopupFactory) : base(stateMachine)
        {
            _modalPopupFactory = modalPopupFactory;
        }

        public async UniTask Enter()
        {
            Application.logMessageReceived += OnApplicationMessageReceived;
            await _stateMachine.NextState();
        }

        private async void OnApplicationMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type is LogType.Error or LogType.Exception)
            {
                var popup = await _modalPopupFactory.Show<ErrorModal>();
                popup.Init(condition, stacktrace);
            }
        }

        public void Dispose()
        {
            Application.logMessageReceived -= OnApplicationMessageReceived;
        }
    }
}