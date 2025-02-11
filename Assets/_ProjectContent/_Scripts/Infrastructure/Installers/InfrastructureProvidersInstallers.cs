using Infrastructure.Providers;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Providers.LoadingCurtainProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureProvidersInstallers : MonoInstaller
    {
        [SerializeField] private LoadingCurtainProvider _loadingCurtainProvider;
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private CachedDefaultUnityRemoteConfigProvider cachedDefaultUnityRemoteConfigProvider;

        public override void InstallBindings()
        {
            BindLoadingCurtainProvider();
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
        
        private void BindLoadingCurtainProvider()
        {
            Container.BindInterfacesTo<LoadingCurtainProvider>().FromInstance(_loadingCurtainProvider).AsSingle().NonLazy();
        }

        private void BindDefaultConfigProvider()
        {
            Container.BindInterfacesTo<CachedDefaultUnityRemoteConfigProvider>().FromInstance(cachedDefaultUnityRemoteConfigProvider).AsSingle().NonLazy();
        }
    }
}