#if !NOT_UNITY3D

using Plugins.Zenject.Source.Injection;
using Plugins.Zenject.Source.Internal;
using UnityEngine;
using Zenject;

namespace Plugins.Zenject.Source.Providers.PrefabProviders
{
    [NoReflectionBaking]
    public class PrefabProviderResource : IPrefabProvider
    {
        readonly string _resourcePath;

        public PrefabProviderResource(string resourcePath)
        {
            _resourcePath = resourcePath;
        }

        public UnityEngine.Object GetPrefab(InjectContext context)
        {
            var prefab = (GameObject)Resources.Load(_resourcePath);

            Assert.That(prefab != null,
                "Expected to find prefab at resource path '{0}'", _resourcePath);

            return prefab;
        }
    }
}

#endif

