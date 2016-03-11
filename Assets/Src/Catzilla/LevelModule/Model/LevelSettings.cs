using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly int ExtraScore;
        public readonly float PlayerFrontSpeed;
        public readonly float PlayerSideSpeed;
        public readonly int ResurrectionReward;

        private readonly IDictionary<LevelObjectType, ObjectLevelSettings> objectSettings;

        public LevelSettings(
            int index,
            int completionScore,
            int extraScore,
            float playerFrontSpeed,
            float playerSideSpeed,
            int resurrectionReward,
            IDictionary<LevelObjectType, ObjectLevelSettings> objectSettings) {
            Index = index;
            CompletionScore = completionScore;
            ExtraScore = extraScore;
            PlayerFrontSpeed = playerFrontSpeed;
            PlayerSideSpeed = playerSideSpeed;
            ResurrectionReward = resurrectionReward;
            this.objectSettings = objectSettings;
        }

        /**
         * @return Can be null
         */
        public ObjectLevelSettings GetObjectSettings(
            LevelObjectType objectType) {
            ObjectLevelSettings result = null;
            objectSettings.TryGetValue(objectType, out result);
            return result;
        }
    }
}
