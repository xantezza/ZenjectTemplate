using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Providers.AudioProvider;
using Infrastructure.Providers.DefaultConfigProvider;
using UnityEngine;
using Utilities.Assertions;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureProvidersInstallers : MonoInstaller
    {
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private AudioProvider _audioProvider;
        [SerializeField] private CachedDefaultUnityRemoteConfigProvider _cachedDefaultUnityRemoteConfigProvider;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AssetReferenceProvider>().FromInstance(_assetReferenceProvider.AssertNotNull()).AsSingle().NonLazy();
            Container.BindInterfacesTo<AudioProvider>().FromInstance(_audioProvider.AssertNotNull()).AsSingle().NonLazy();
            Container.BindInterfacesTo<CachedDefaultUnityRemoteConfigProvider>().FromInstance(_cachedDefaultUnityRemoteConfigProvider.AssertNotNull()).AsSingle().NonLazy();
        }

        private void Awake()
        {
            _assetReferenceProvider.ValidateReferences();
        }
        
        private void OnValidate()
        {
            _cachedDefaultUnityRemoteConfigProvider.Validate();
        }
    }
}