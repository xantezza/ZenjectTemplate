using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
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
            BindStatesFactory();
            BindGameLoopStateMachineFactory();
            BindInitializationStateMachineFactory();
        }

        private void BindConditionalLoggingService() => Container.Bind<IConditionalLoggingService>().To<UnityConditionalLoggingService>().FromNew().AsSingle().NonLazy();
        private void BindCoroutineRunnerService() => Container.BindInterfacesTo<CoroutineRunnerService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        private void BindSceneLoaderService() => Container.BindInterfacesTo<SceneLoaderService>().FromNew().AsSingle().NonLazy();
        private void BindAnalyticsLogService() => Container.Bind<IAnalyticsLogService>().To<AnalyticsLogService>().FromNew().AsSingle().NonLazy();
        private void BindStatesFactory() => Container.Bind<StatesFactory>().FromNew().AsSingle().NonLazy();

        private void BindGameLoopStateMachineFactory()
        {
            Container.Bind<GameLoopStateMachineFactory>().FromNew().AsSingle().NonLazy();
        }

        private void BindInitializationStateMachineFactory()
        {
            Container.Bind<InitializationStateMachineFactory>().FromNew().AsSingle().NonLazy();
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