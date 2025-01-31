using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Providers.DefaultConfigProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureProvidersInstallers : MonoInstaller
    {
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private CachedDefaultUnityRemoteConfigProvider cachedDefaultUnityRemoteConfigProvider;

        public override void InstallBindings()
        {
            BindAssetReferenceProvider();
            BindDefaultConfigProvider();
        }

        private void Awake()
        {
            _assetReferenceProvider.ValidateReferences();
        }

        private void BindAssetReferenceProvider()
        {
            Container.BindInterfacesTo<AssetReferenceProvider>().FromInstance(_assetReferenceProvider).AsSingle().NonLazy();
        }

        private void BindDefaultConfigProvider()
        {
            Container.BindInterfacesTo<CachedDefaultUnityRemoteConfigProvider>().FromInstance(cachedDefaultUnityRemoteConfigProvider).AsSingle().NonLazy();
        }
    }
}