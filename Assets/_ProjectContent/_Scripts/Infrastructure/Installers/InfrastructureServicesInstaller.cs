using Infrastructure.Services.Analytics;
using Infrastructure.Services.AudioService;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.Services.SettingsService;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureServicesInstaller : MonoInstaller
    {
        [SerializeField] private AudioService _audioService;
        [SerializeField] private SettingsService _settingsService;
        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalyticsLogService();
            BindSaveService();
        }

        private void BindConditionalLoggingService()
        {
            Container.Bind<ConditionalLoggingService>().To<UnityConditionalLoggingService>().FromNew().AsSingle().NonLazy();
        }
        private void BindAnalyticsLogService()
        {
            Container.BindInterfacesTo<UnityAnalyticsService>().FromNew().AsSingle().NonLazy();
        }

        private void BindCoroutineRunnerService()
        {
            Container.BindInterfacesTo<CoroutineRunnerService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void BindSceneLoaderService()
        {
            Container.BindInterfacesTo<SceneLoaderService>().FromNew().AsSingle().NonLazy();
        }

        private void BindSaveService()
        {
#if DEV
            // Readable
            Container.BindInterfacesTo<JsonSaveService>().FromNew().AsSingle().NonLazy();
#else
            // Encrypted
            Container.BindInterfacesTo<BinarySaveService>().FromNew().AsSingle().NonLazy();
#endif
        }
    }
}