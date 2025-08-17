using Infrastructure.Services.Analytics;
using Infrastructure.Services.Audio;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.Services.Settings;
using Infrastructure.Services.Windows;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureServicesInstaller : MonoInstaller
    {
        [SerializeField] private AudioService _audioService;
        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalyticsLogService();
            BindSaveService();
            BindAudioService();
            BindSettingsService();
            BindWindowService();
        }

        private void BindConditionalLoggingService()
        {
            Container.Bind<LoggingService>().To<UnityLoggingService>().FromNew().AsSingle().NonLazy();
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
        
        private void BindAudioService()
        {
            Container.BindInterfacesTo<AudioService>().FromInstance(_audioService).AsSingle().NonLazy();
        }
        
        private void BindSettingsService()
        {
            Container.BindInterfacesTo<SettingsService>().FromNew().AsSingle().NonLazy();
        }
        private void BindWindowService()
        {
            Container.BindInterfacesTo<WindowService>().FromNew().AsSingle().NonLazy();
        }
    }
}