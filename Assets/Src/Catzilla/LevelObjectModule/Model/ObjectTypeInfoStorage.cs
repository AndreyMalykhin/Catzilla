using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [CreateAssetMenuAttribute]
    public class ObjectTypeInfoStorage: ScriptableObject {
        [System.Serializable]
        private class Item {
            public LevelObjectType Type = 0;
            public ObjectTypeInfo TypeInfo = null;
        }

        [SerializeField]
        private Item[] items;

        private readonly IDictionary<LevelObjectType, ObjectTypeInfo> itemsMap =
            new Dictionary<LevelObjectType, ObjectTypeInfo>();

        public ObjectTypeInfo Get(LevelObjectType type) {
            return itemsMap[type];
        }

        private void OnEnable() {
            for (int i = 0; i < items.Length; ++i) {
                itemsMap[items[i].Type] = items[i].TypeInfo;
            }
        }
    }
}
