#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Injection;
using Plugins.Zenject.Source.Internal;
using Plugins.Zenject.Source.Main;
using UnityEngine;
using Zenject;

namespace Plugins.Zenject.Source.Providers.GameObjectProviders
{
    [NoReflectionBaking]
    public class EmptyGameObjectProvider : IProvider
    {
        readonly DiContainer _container;
        readonly GameObjectCreationParameters _gameObjectBindInfo;

        public EmptyGameObjectProvider(
            DiContainer container, GameObjectCreationParameters gameObjectBindInfo)
        {
            _gameObjectBindInfo = gameObjectBindInfo;
            _container = container;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return typeof(GameObject);
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);

            injectAction = null;

            var gameObj = _container.CreateEmptyGameObject(_gameObjectBindInfo, context);
            buffer.Add(gameObj);
        }
    }
}

#endif

