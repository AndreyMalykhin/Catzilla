using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly int AreaDangerousObjects;
        public readonly int AreaScoreableObjects;
        public readonly float PlayerFrontSpeed;
        public readonly float PlayerSideSpeed;
        public readonly int ResurrectionReward;

        public LevelSettings(
            int index,
            int completionScore,
            int areaDangerousObjects,
            int areaScoreableObjects,
            float playerFrontSpeed,
            float playerSideSpeed,
            int resurrectionReward) {
            Index = index;
            CompletionScore = completionScore;
            AreaDangerousObjects = areaDangerousObjects;
            AreaScoreableObjects = areaScoreableObjects;
            PlayerFrontSpeed = playerFrontSpeed;
            PlayerSideSpeed = playerSideSpeed;
            ResurrectionReward = resurrectionReward;
        }
    }
}
