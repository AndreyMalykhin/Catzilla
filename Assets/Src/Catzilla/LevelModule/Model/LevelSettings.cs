using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly int ExtraScore;
        public readonly float PlayerFrontSpeed;
        public readonly float PlayerSideSpeed;
        public readonly int ResurrectionReward;

        public LevelSettings(
            int index,
            int completionScore,
            int extraScore,
            float playerFrontSpeed,
            float playerSideSpeed,
            int resurrectionReward) {
            Index = index;
            CompletionScore = completionScore;
            ExtraScore = extraScore;
            PlayerFrontSpeed = playerFrontSpeed;
            PlayerSideSpeed = playerSideSpeed;
            ResurrectionReward = resurrectionReward;
        }
    }
}
