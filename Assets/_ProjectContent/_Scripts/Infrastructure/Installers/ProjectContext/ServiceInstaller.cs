using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Modals;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.InitializationStateMachine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalyticsLogService();
            BindSaveService();
            BindFactories();
        }

        private void BindFactories()
        {
            Container.Bind<StatesFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<GameLoopStateMachineFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<InitializationStateMachineFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<ModalsFactory>().FromNew().AsSingle().NonLazy();
        }

        private void BindConditionalLoggingService()
        {
            Container.BindInterfacesTo<UnityConditionalLoggingService>().FromNew().AsSingle().NonLazy();
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