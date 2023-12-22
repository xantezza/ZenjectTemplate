using Infrastructure.Services.Analytics;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalyticsService();
            BindSaveService();
        }

        private void BindConditionalLoggingService()
        {
            Container.Bind<IConditionalLoggingService>().To<UnityConditionalLoggingService>().FromNew().AsSingle();
        }

        private void BindCoroutineRunnerService()
        {
            Container.BindInterfacesTo<CoroutineRunnerService>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindSceneLoaderService()
        {
            Container.BindInterfacesTo<SceneLoaderService>().FromNew().AsSingle();
        }

        private void BindAnalyticsService()
        {
            Container.Bind<IAnalyticsLogService>().To<FirebaseAnalyticsLogService>().FromNew().AsSingle();
            Container.Bind<Analytics>().To<Analytics>().FromNew().AsSingle();
        }

        private void BindSaveService()
        {
#if DEV
            Container.BindInterfacesTo<JsonSaveService>().FromNew().AsSingle();
#else
            Container.BindInterfacesTo<BinarySaveService>().FromNew().AsSingle();
#endif
        }
    }
}