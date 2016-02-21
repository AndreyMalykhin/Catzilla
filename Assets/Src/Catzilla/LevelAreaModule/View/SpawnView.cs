using UnityEngine;
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

        public LevelObjectType ObjectType {get {return objectType;}}

        [SerializeField]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

        [SerializeField]
        private LevelObjectType objectType;

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
    }
}
