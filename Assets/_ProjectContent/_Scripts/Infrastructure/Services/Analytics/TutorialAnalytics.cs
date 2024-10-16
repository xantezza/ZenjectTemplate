using Infrastructure.Services.Saving;
using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class Save
    {
        public bool FirstOpen = true;
    }

    public class TutorialAnalytics : IDataSaveable<Save>
    {
        private readonly IAnalyticsSendService _analyticsSendService;

        public SaveKey SaveId => SaveKey.TutorialAnalytics;

        public Save SaveData { get; set; }

        [Inject]
        public TutorialAnalytics(IAnalyticsSendService analyticsSendService, ISaveService saveService)
        {
            _analyticsSendService = analyticsSendService;
            saveService.Process(this);
            Initialize();
        }

        private void Initialize()
        {
            CheckFirstOpen();
        }

        private void CheckFirstOpen()
        {
            if (!SaveData.FirstOpen) return;
            SaveData.FirstOpen = false;
            _analyticsSendService.SendEvent(EventNames.TutorialFirstOpen);
        }
    }
}