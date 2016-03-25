using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    [CreateAssetMenuAttribute]
    public class LevelSettingsStorage: ScriptableObject {
        [Serializable]
        private struct LevelSettingsParams {
            public ObjectLevelSettings[] ObjectSettings;
        }

        [SerializeField]
        private int completionScoreLevelFactor = 512;

        [SerializeField]
        private int minCompletionScore = 2048;

        [SerializeField]
        private float playerMinFrontSpeed = 4f;

        [SerializeField]
        private float playerMaxFrontSpeed = 8f;

        [SerializeField]
        private float playerFrontSpeedLevelFactor = 0.1f;

        [SerializeField]
        private float playerMinSideSpeed = 4f;

        [SerializeField]
        private float playerMaxSideSpeed = 8f;

        [SerializeField]
        private float playerSideSpeedLevelFactor = 0.1f;

        [SerializeField]
        private int resurrectionMinReward = 1;

        [SerializeField]
        private float resurrectionRewardLevelFactor = 0.2f;

        [SerializeField]
        private LevelSettingsParams[] itemsParams;

        [NonSerialized]
        private readonly IDictionary<int, LevelSettings> items =
            new Dictionary<int, LevelSettings>(16);

        public LevelSettings Get(int levelIndex) {
            LevelSettings levelSettings = null;

            if (!items.TryGetValue(levelIndex, out levelSettings)) {
                levelSettings = MakeItem(levelIndex);
                items.Add(levelIndex, levelSettings);
            }

            return levelSettings;
        }

        private LevelSettings MakeItem(int levelIndex) {
            float extraScore = levelIndex * completionScoreLevelFactor;
            float completionScore = minCompletionScore + extraScore;
            float resurrectionReward = resurrectionMinReward + levelIndex *
                resurrectionRewardLevelFactor;
            float playerFrontSpeed = Mathf.Min(playerMinFrontSpeed +
                levelIndex * playerFrontSpeedLevelFactor, playerMaxFrontSpeed);
            float playerSideSpeed = Mathf.Min(playerMinSideSpeed +
                levelIndex * playerSideSpeedLevelFactor, playerMaxSideSpeed);

            if (levelIndex >= itemsParams.Length) {
                levelIndex = itemsParams.Length - 1;
            }

            ObjectLevelSettings[] objectSettings =
                itemsParams[levelIndex].ObjectSettings;
            var objectSettingsMap =
                new Dictionary<int, ObjectLevelSettings>(16);

            for (int i = 0; i < objectSettings.Length; ++i) {
                objectSettingsMap.Add(
                    (int) objectSettings[i].ObjectType, objectSettings[i]);
            }

            return new LevelSettings(
                levelIndex,
                (int) completionScore,
                (int) extraScore,
                playerFrontSpeed,
                playerSideSpeed,
                (int) resurrectionReward,
                objectSettingsMap);
        }

        private void OnEnable() {
            for (int i = 0; i < itemsParams.Length; ++i) {
                items.Add(i, MakeItem(i));
            }
        }
    }
}
