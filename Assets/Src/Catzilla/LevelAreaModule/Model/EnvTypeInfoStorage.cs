using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    [CreateAssetMenuAttribute]
    public class EnvTypeInfoStorage: ScriptableObject {
        [SerializeField]
        private EnvTypeInfo[] items;

        [NonSerialized]
        private readonly IDictionary<int, EnvTypeInfo> itemsMap =
            new Dictionary<int, EnvTypeInfo>();

        public EnvTypeInfo Get(EnvType envType) {
            return itemsMap[(int) envType];
        }

        private void OnEnable() {
            // DebugUtils.Log("EnvTypeInfoStorage.OnEnable()");

            for (var i = 0; i < items.Length; ++i) {
                itemsMap.Add((int) items[i].Type, items[i]);
            }
        }
    }
}
