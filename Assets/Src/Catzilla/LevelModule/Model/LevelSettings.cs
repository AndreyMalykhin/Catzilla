using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    public class LevelSettings {
        public readonly int Index;
        public readonly int CompletionScore;
        public readonly int ExtraScore;
        public readonly int ResurrectionReward;

        private readonly IDictionary<int, ObjectLevelSettings> objectSettings;

        public LevelSettings(
            int index,
            int completionScore,
            int extraScore,
            int resurrectionReward,
            IDictionary<int, ObjectLevelSettings> objectSettings) {
            Index = index;
            CompletionScore = completionScore;
            ExtraScore = extraScore;
            ResurrectionReward = resurrectionReward;
            this.objectSettings = objectSettings;
        }

        /**
         * @return Can be null
         */
        public ObjectLevelSettings GetObjectSettings(
            LevelObjectType objectType) {
            ObjectLevelSettings result = null;
            objectSettings.TryGetValue((int) objectType, out result);
            return result;
        }
    }
}
