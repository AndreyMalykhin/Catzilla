using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    public class SpawnView: MonoBehaviour, ISerializationCallbackReceiver {
        public SpawnLocation Location {
            get {
                return new SpawnLocation(
                    new Bounds(transform.position, size), isXFlipped);
            }
        }

        public List<LevelObjectType> ObjectTypes {
            get {
                var result = new List<LevelObjectType>(extraObjectTypes);
                result.Add(objectType);
                return result;
            }
        }

        [SerializeField]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

        [SerializeField]
        private LevelObjectType objectType;

        [SerializeField]
        private LevelObjectType[] extraObjectTypes;

        [SerializeField]
        private int itemsAcrossX;

        [SerializeField]
        private int itemsAcrossZ;

        [SerializeField]
        private Color color;

        [SerializeField]
        private bool isXFlipped;

        [SerializeField]
        [HideInInspector]
        private Vector3 size;

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            // DebugUtils.Log("SpawnView.OnAfterDeserialize()");
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            ValidateExtraObjectTypes();
            size = GetSize();
        }

        private void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, GetSize());
        }

        private Vector3 GetSize() {
            ObjectTypeInfo objectTypeInfo =
                objectTypeInfoStorage.Get(objectType);
            return new Vector3(
                itemsAcrossX * objectTypeInfo.Width,
                0f,
                itemsAcrossZ * objectTypeInfo.Depth);
        }

        private void ValidateExtraObjectTypes() {
            ObjectTypeInfo objectTypeInfo =
                objectTypeInfoStorage.Get(objectType);

            for (int i = 0; i < extraObjectTypes.Length; ++i) {
                ObjectTypeInfo extraObjectTypeInfo =
                    objectTypeInfoStorage.Get(extraObjectTypes[i]);
                DebugUtils.Assert(
                    objectTypeInfo.Width == extraObjectTypeInfo.Width);
                DebugUtils.Assert(
                    objectTypeInfo.Depth == extraObjectTypeInfo.Depth);
            }
        }
    }
}
