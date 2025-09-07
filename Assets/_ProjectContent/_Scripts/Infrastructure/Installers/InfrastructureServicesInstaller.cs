using Infrastructure.Services.Analytics;
using Infrastructure.Services.Audio;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.LoadingCurtain;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.Services.Settings;
using Infrastructure.Services.Windows;
using UnityEngine;
using Utilities.Assertions;
using Zenject;
using Logger = Infrastructure.Services.Log.Logger;

namespace Infrastructure.Installers
{
    public class InfrastructureServicesInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtainService loadingCurtainService;
        [SerializeField] private AudioService _audioService;

        public override void InstallBindings()
        {
            Container.Bind<Logger>().To<Logger>().FromNew().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<UnityAnalyticsService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<CoroutineRunnerService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneLoaderService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<WindowsService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<SettingsService>().FromNew().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<LoadingCurtainService>().FromInstance(loadingCurtainService.AssertNotNull()).AsSingle().NonLazy();
            Container.BindInterfacesTo<AudioService>().FromInstance(_audioService.AssertNotNull()).AsSingle().NonLazy();
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