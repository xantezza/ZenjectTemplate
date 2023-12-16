using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Plugins.Zenject.Source.Install;
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

        private void BindSaveService()
        {
            Container.BindInterfacesTo<PlayerPrefsSaveService>().FromNew().AsSingle();
        }
    }
}