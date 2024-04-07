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
            var debugRoot = await _assetReferenceProvider.DebugRootAssetReference.InstantiateAsync();
            Object.DontDestroyOnLoad(debugRoot);

            await ToNextState();
        }
    }
}