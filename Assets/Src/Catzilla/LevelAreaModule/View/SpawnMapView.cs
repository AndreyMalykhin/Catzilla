using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    public class SpawnMapView: MonoBehaviour {
        public SpawnView[] Items {get {return spawns;}}

        [SerializeField]
        private SpawnView[] spawns;

        [SerializeField]
        private int width;

        [SerializeField]
        private int depth;

        private void OnDrawGizmos() {
            Gizmos.color = Color.grey;

            for (int x = -width / 2; x <= width / 2; ++x) {
                Gizmos.DrawLine(
                    new Vector3(x, 0f, -depth / 2),
                    new Vector3(x, 0f, depth / 2));
            }

            for (int z = -depth / 2; z <= depth / 2; ++z) {
                Gizmos.DrawLine(
                    new Vector3(-width / 2, 0f, z),
                    new Vector3(width / 2, 0f, z));
            }
        }
    }
}
