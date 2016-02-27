using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    public struct SpawnsInfo {
        public readonly LevelObjectType ObjectType;
        public readonly int Count;

        public SpawnsInfo(LevelObjectType objectType, int count) {
            ObjectType = objectType;
            Count = count;
        }
    }
}
