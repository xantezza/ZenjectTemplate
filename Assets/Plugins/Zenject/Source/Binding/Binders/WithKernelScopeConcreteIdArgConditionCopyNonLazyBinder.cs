using Plugins.Zenject.Source.Providers.SubContainerCreators;
using Plugins.Zenject.Source.Runtime.Kernels;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders
{
    [NoReflectionBaking]
    public class WithKernelScopeConcreteIdArgConditionCopyNonLazyBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        SubContainerCreatorBindInfo _subContainerBindInfo;

        public WithKernelScopeConcreteIdArgConditionCopyNonLazyBinder(
            SubContainerCreatorBindInfo subContainerBindInfo, BindInfo.BindInfo bindInfo)
            : base(bindInfo)
        {
            _subContainerBindInfo = subContainerBindInfo;
        }

        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel()
        {
            _subContainerBindInfo.CreateKernel = true;
            return this;
        }

        // This would be used in cases where you want to control the execution order for the
        // subcontainer
        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel<TKernel>()
            where TKernel : Kernel
        {
            _subContainerBindInfo.CreateKernel = true;
            _subContainerBindInfo.KernelType = typeof(TKernel);
            return this;
        }
    }
}
