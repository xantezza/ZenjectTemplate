using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Modals;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using UniRx;
using Unity.Services.Analytics;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializePrivacyPolicyState : BaseInitializationState, IDataSaveable<InitializePrivacyPolicyState.Save>, IEnterableState
    {
        [Serializable]
        public class Save
        {
            public bool ConsentGiven;
        }

        public SaveKey SaveId => SaveKey.PrivacyPolicyState;
        public Save SaveData { get; set; }

        private readonly ISaveService _saveService;
        private readonly ModalsFactory _modalsFactory;

        protected InitializePrivacyPolicyState(InitializationStateMachine stateMachine, ModalsFactory modalsFactory, ISaveService saveService) : base(stateMachine)
        {
            _modalsFactory = modalsFactory;
            _saveService = saveService;
        }

        public async UniTask Enter()
        {
            _saveService.Process(this);

            if (SaveData.ConsentGiven)
            {
                if (InitializeUnityServicesState.IsInitialized) AnalyticsService.Instance.StartDataCollection();
                await _stateMachine.NextState();
            }
            else
            {
                var popup = await _modalsFactory.Show<PrivacyPolicyModal>();
                popup.OnInteract.Subscribe(_ => OnInteract());
            }
        }

        private async void OnInteract()
        {
            SaveData.ConsentGiven = true;
            if (InitializeUnityServicesState.IsInitialized) AnalyticsService.Instance.StartDataCollection();
            await _stateMachine.NextState();
        }
    }
}