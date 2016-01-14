using UnityEngine;
using System.Collections;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    public class ObjectTypeInfo {
        public int Width {get; private set;}
        public int Depth {get; private set;}
        public int SpawnPriority {get; private set;}
        public float SpawnChance {get; private set;}
        public int SpawnBaseCount {get; private set;}
        public float SpawnLevelFactor {get; private set;}
        public LevelObjectView ViewProto {get; private set;}

        public ObjectTypeInfo(
            int width,
            int depth,
            int spawnPriority,
            float spawnChance,
            int spawnBaseCount,
            float spawnLevelFactor,
            LevelObjectView viewProto) {
            Width = width;
            Depth = depth;
            SpawnPriority = spawnPriority;
            SpawnChance = spawnChance;
            SpawnBaseCount = spawnBaseCount;
            SpawnLevelFactor = spawnLevelFactor;
            ViewProto = viewProto;
        }
    }
}
