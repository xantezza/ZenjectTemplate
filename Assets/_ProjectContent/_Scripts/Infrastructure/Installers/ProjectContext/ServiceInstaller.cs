using Infrastructure.Services.Analytics;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.OnGuiDev;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private DevGUIService _devGUIService;

        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindDevGUIService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalyticsLogService();
            BindSaveService();
        }

        private void BindConditionalLoggingService()
        {
            Container.Bind<ConditionalLoggingService>().To<UnityConditionalLoggingService>().FromNew().AsSingle().NonLazy();
        }

        private void BindDevGUIService()
        {
            Container.BindInterfacesTo<DevGUIService>().FromInstance(_devGUIService).AsSingle().NonLazy();
        }

        private void BindAnalyticsLogService()
        {
            Container.BindInterfacesTo<UnityAnalyticsSendService>().FromNew().AsSingle().NonLazy();
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
            Container.BindInterfacesTo<JsonSaveService>().FromNew().AsSingle();
#else
            // Faster
            Container.BindInterfacesTo<BinarySaveService>().FromNew().AsSingle();
#endif
        }
    }
}