using UnityEngine;

namespace Catzilla.LevelAreaModule.Model {
    public struct SpawnLocation {
        public readonly Bounds Bounds;
        public readonly bool IsXFlipped;

        public SpawnLocation(Bounds bounds, bool isXFlipped) {
            Bounds = bounds;
            IsXFlipped = isXFlipped;
        }
    }
}
