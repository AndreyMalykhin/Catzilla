using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly float PlayerFrontSpeed;
        public readonly float PlayerSideSpeed;
        public readonly int ResurrectionReward;

        public LevelSettings(
            int index,
            int completionScore,
            float playerFrontSpeed,
            float playerSideSpeed,
            int resurrectionReward) {
            Index = index;
            CompletionScore = completionScore;
            PlayerFrontSpeed = playerFrontSpeed;
            PlayerSideSpeed = playerSideSpeed;
            ResurrectionReward = resurrectionReward;
        }
    }
}
