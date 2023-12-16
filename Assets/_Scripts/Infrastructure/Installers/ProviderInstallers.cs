using Infrastructure.Providers;
using Infrastructure.Providers.DefaultConfigProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProviderInstallers : MonoInstaller
    {
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private CachedDefaultConfigProvider cachedDefaultConfigProvider;

        public override void InstallBindings()
        {
            BindGameLoopStateMachineProvider();
            BindAssetReferenceProvider();
            BindDefaultConfigProvider();
        }

        private void BindGameLoopStateMachineProvider()
        {
            Container.Bind<GameLoopStateMachineProvider>().FromNew().AsSingle();
        }

        private void BindAssetReferenceProvider()
        {
            Container.Bind<AssetReferenceProvider>().FromInstance(_assetReferenceProvider).AsSingle();
        }

        private void BindDefaultConfigProvider()
        {
            Container.BindInterfacesTo<CachedDefaultConfigProvider>().FromInstance(cachedDefaultConfigProvider).AsSingle();
        }
    }
}