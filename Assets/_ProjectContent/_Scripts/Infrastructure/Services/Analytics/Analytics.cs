using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class Analytics
    {
        private readonly IInstantiator _instantiator;
        public readonly TutorialAnalytics TutorialAnalytics;

        [Inject]
        public Analytics(IInstantiator instantiator)
        {
            _instantiator = instantiator;
            TutorialAnalytics = instantiator.Instantiate<TutorialAnalytics>();
        }
    }
}