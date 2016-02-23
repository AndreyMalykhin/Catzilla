using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly int AreaDangerousObjects;
        public readonly int AreaScoreableObjects;
        public readonly int BonusObjects;
        public readonly float BonusObjectSpawnChance;
        public readonly float PlayerFrontSpeed;
        public readonly float PlayerSideSpeed;
        public readonly int ResurrectionReward;

        public LevelSettings(
            int index,
            int completionScore,
            int areaDangerousObjects,
            int areaScoreableObjects,
            int bonusObjects,
            float bonusObjectSpawnChance,
            float playerFrontSpeed,
            float playerSideSpeed,
            int resurrectionReward) {
            Index = index;
            CompletionScore = completionScore;
            AreaDangerousObjects = areaDangerousObjects;
            AreaScoreableObjects = areaScoreableObjects;
            BonusObjects = bonusObjects;
            BonusObjectSpawnChance = bonusObjectSpawnChance;
            PlayerFrontSpeed = playerFrontSpeed;
            PlayerSideSpeed = playerSideSpeed;
            ResurrectionReward = resurrectionReward;
        }
    }
}
