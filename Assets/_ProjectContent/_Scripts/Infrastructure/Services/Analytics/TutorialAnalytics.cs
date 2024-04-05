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
        private readonly IAnalyticsLogService _analyticsLogService;

        public string SaveId => SaveKeys.TutorialAnalytics;
        
        public Save SaveData { get; set; }

        [Inject]
        public TutorialAnalytics(IAnalyticsLogService analyticsLogService, ISaveService saveService)
        {
            _analyticsLogService = analyticsLogService;
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
            _analyticsLogService.LogEvent(EventNames.TutorialFirstOpen);
        }
    }
}