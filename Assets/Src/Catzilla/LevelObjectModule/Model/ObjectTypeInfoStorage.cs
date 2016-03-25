using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [CreateAssetMenuAttribute]
    public class ObjectTypeInfoStorage: ScriptableObject {
        [SerializeField]
        private ObjectTypeInfo[] items;

        [NonSerialized]
        private readonly IDictionary<int, ObjectTypeInfo> itemsMap =
            new Dictionary<int, ObjectTypeInfo>(16);

        public ObjectTypeInfo Get(LevelObjectType type) {
            return itemsMap[(int) type];
        }

        public ICollection<ObjectTypeInfo> GetAll() {
            return itemsMap.Values;
        }

        private void OnEnable() {
            for (int i = 0; i < items.Length; ++i) {
                itemsMap.Add((int) items[i].Type, items[i]);
            }
        }
    }
}
