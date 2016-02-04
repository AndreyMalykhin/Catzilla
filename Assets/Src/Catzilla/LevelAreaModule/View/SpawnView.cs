using UnityEngine;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.View {
    [ExecuteInEditMode]
    public class SpawnView: MonoBehaviour {
        public Bounds Location {
            get {
                return new Bounds(transform.position, GetSize());
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
