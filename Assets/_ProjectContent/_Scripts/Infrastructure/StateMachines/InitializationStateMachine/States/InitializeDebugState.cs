using Cysharp.Threading.Tasks;
using Infrastructure.Providers;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeDebugState : BaseInitializationState, IEnterableState
    {
        private static bool IsInitialized { get; set; }

        private readonly AssetReferenceProvider _assetReferenceProvider;
        
        [Inject]
        public InitializeDebugState(InitializationStateMachine stateMachine, AssetReferenceProvider assetReferenceProvider) : base(stateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }

        public async UniTask Enter()
        {
            await InitializeDebugRoot();
        }

        private async UniTask InitializeDebugRoot()
        {
            if (IsInitialized)
            {
                await ToNextState();
                return;
            }

            IsInitialized = true;

#if DEV
            var debugRoot = await _assetReferenceProvider.DebugRootAssetReference.InstantiateAsync();
            Object.DontDestroyOnLoad(debugRoot);
#endif

            await ToNextState();
        }
    }
}