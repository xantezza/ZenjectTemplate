using System;
using Plugins.Zenject.Source.Injection;
using Plugins.Zenject.Source.Internal;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders
{
    [NoReflectionBaking]
    public class InstantiateCallbackConditionCopyNonLazyBinder : ConditionCopyNonLazyBinder
    {
        public InstantiateCallbackConditionCopyNonLazyBinder(BindInfo.BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConditionCopyNonLazyBinder OnInstantiated(
            Action<InjectContext, object> callback)
        {
            BindInfo.InstantiatedCallback = callback;
            return this;
        }

        public ConditionCopyNonLazyBinder OnInstantiated<T>(
            Action<InjectContext, T> callback)
        {
            // Can't do this here because of factory bindings
            //Assert.That(BindInfo.ContractTypes.All(x => x.DerivesFromOrEqual<T>()));

            BindInfo.InstantiatedCallback = (ctx, obj) =>
            {
                Assert.That(obj == null || obj is T,
                    "Invalid generic argument to OnInstantiated! {0} must be type {1}", obj.GetType(), typeof(T));

                callback(ctx, (T)obj);
            };
            return this;
        }
    }
}
