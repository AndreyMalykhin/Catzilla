using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    [ExecuteInEditMode]
    public class SpawnView: MonoBehaviour {
        public SpawnLocation Location {
            get {
                return new SpawnLocation(
                    new Bounds(transform.position, GetSize()), isXFlipped);
            }
        }

        public List<LevelObjectType> ObjectTypes {
            get {
                CheckExtraObjectTypes();
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
        private int itemsAcrossX = 1;

        [SerializeField]
        private int itemsAcrossZ = 1;

        [SerializeField]
        private Color color = Color.green;

        [SerializeField]
        private bool isXFlipped;

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

        private void CheckExtraObjectTypes() {
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
