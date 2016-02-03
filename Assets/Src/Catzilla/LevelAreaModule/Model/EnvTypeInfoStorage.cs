using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    [CreateAssetMenuAttribute]
    public class EnvTypeInfoStorage: ScriptableObject {
        [SerializeField]
        private EnvTypeInfo[] items;

        private readonly IDictionary<EnvType, EnvTypeInfo> itemsMap =
            new Dictionary<EnvType, EnvTypeInfo>();

        public EnvTypeInfo Get(EnvType envType) {
            return itemsMap[envType];
        }

        private void OnEnable() {
            // DebugUtils.Log("EnvTypeInfoStorage.OnEnable()");

            for (var i = 0; i < items.Length; ++i) {
                itemsMap.Add(items[i].Type, items[i]);
            }
        }
    }
}
