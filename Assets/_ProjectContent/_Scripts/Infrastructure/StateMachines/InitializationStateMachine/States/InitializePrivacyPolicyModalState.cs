using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Services.Log;
using Infrastructure.Services.Modals;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using UniRx;
using Unity.Services.Analytics;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializePrivacyPolicyModalState : BaseInitializationState, IDataSaveable<InitializePrivacyPolicyModalState.Save>, IEnterableState
    {
        private const bool EDITOR_IGNORE_CONSENT = false;
        private const bool IGNORE_CONSENT = false;

        [Serializable]
        public class Save
        {
            public bool ConsentGiven = false;
        }

        public SaveKey SaveKey => SaveKey.PrivacyPolicyState;
        public Save SaveData { get; private set; }

        private readonly ISaveService _saveService;
        private readonly IModalPopupFactory _modalPopupFactory;

        protected InitializePrivacyPolicyModalState(InitializationStateMachine stateMachine, IModalPopupFactory modalPopupFactory, ISaveService saveService) : base(stateMachine)
        {
            _modalPopupFactory = modalPopupFactory;
            _saveService = saveService;
        }

        public async UniTask Enter()
        {
            SaveData = _saveService.Load(this) ?? new Save();
            _saveService.AddToSaveables(this);

#if UNITY_EDITOR
            SaveData.ConsentGiven = EDITOR_IGNORE_CONSENT;
#else
            if (IGNORE_CONSENT) SaveData.ConsentGiven = IGNORE_CONSENT;
#endif
            if (SaveData.ConsentGiven)
            {
                if (InitializeUnityServicesState.IsInitialized) AnalyticsService.Instance.StartDataCollection();
                await _stateMachine.NextState();
            }
            else
            {
                var popup = await _modalPopupFactory.Show<PrivacyPolicyModal>();
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