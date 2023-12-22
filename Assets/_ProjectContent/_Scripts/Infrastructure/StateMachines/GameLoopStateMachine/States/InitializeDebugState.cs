using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    [UsedImplicitly]
    public class InitializeDebugState : BaseState, IEnterableState
    {
        private static bool IsInitialized { get; set; }

        private readonly GameLoopStateMachine _gameLoopStateMachine;
        private readonly AssetReferenceProvider _assetReferenceProvider;

        public override string StateName => nameof(InitializeDebugState);

        public InitializeDebugState(GameLoopStateMachine stateMachine, AssetReferenceProvider assetReferenceProvider)
        {
            _gameLoopStateMachine = stateMachine;
            _assetReferenceProvider = assetReferenceProvider;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeSaveServiceState>();
        }

        public async void Enter()
        {
            await InitializeDebugRoot();
        }

        private async UniTask InitializeDebugRoot()
        {
            if (IsInitialized)
            {
                ToNextState();
                return;
            }

            IsInitialized = true;
#if DEV
            var debugRoot = await _assetReferenceProvider.DebugRootAssetReference.InstantiateAsync();
            Object.DontDestroyOnLoad(debugRoot);
#endif
            ToNextState();
        }
    }
}