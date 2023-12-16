using Plugins.Zenject.Source.Binding.Binders.Factory.FactoryArgumentsToChoiceBinder;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Main;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders.Factory.Pooling
{
    [NoReflectionBaking]
    public class MemoryPoolMaxSizeBinder<TContract> : MemoryPoolExpandBinder<TContract>
    {
        public MemoryPoolMaxSizeBinder(
            DiContainer bindContainer, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolExpandBinder<TContract> WithMaxSize(int size)
        {
            MemoryPoolBindInfo.MaxSize = size;
            return this;
        }
    }

    [NoReflectionBaking]
    public class MemoryPoolInitialSizeMaxSizeBinder<TContract> : MemoryPoolMaxSizeBinder<TContract>
    {
        public MemoryPoolInitialSizeMaxSizeBinder(
            DiContainer bindContainer, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolMaxSizeBinder<TContract> WithInitialSize(int size)
        {
            MemoryPoolBindInfo.InitialSize = size;
            return this;
        }

        public FactoryArgumentsToChoiceBinder<TContract> WithFixedSize(int size)
        {
            MemoryPoolBindInfo.InitialSize = size;
            MemoryPoolBindInfo.MaxSize = size;
            MemoryPoolBindInfo.ExpandMethod = PoolExpandMethods.Disabled;
            return this;
        }
    }

    [NoReflectionBaking]
    public class MemoryPoolIdInitialSizeMaxSizeBinder<TContract> : MemoryPoolInitialSizeMaxSizeBinder<TContract>
    {
        public MemoryPoolIdInitialSizeMaxSizeBinder(
            DiContainer bindContainer, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        public MemoryPoolInitialSizeMaxSizeBinder<TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}

