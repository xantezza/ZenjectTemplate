using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeDebugToolsState : BaseInitializationState, IEnterableState
    {
        private readonly IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public InitializeDebugToolsState(InitializationStateMachine stateMachine, IAssetReferenceProvider assetReferenceProvider) : base(stateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask Enter()
        {
            var debugRoot = await _assetReferenceProvider.DebugRootAssetReference.InstantiateAsync();
            Object.DontDestroyOnLoad(debugRoot);
            await ToNextState();
        }
        
        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }
    }
}