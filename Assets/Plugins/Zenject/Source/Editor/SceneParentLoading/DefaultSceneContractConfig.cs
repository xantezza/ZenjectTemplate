using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.Zenject.Source.Editor.SceneParentLoading
{
    public class DefaultSceneContractConfig : ScriptableObject
    {
        public const string ResourcePath = "ZenjectDefaultSceneContractConfig";

        public List<ContractInfo> DefaultContracts;

        [Serializable]
        public class ContractInfo
        {
            public string ContractName;
            public SceneAsset Scene;
        }
    }

}
