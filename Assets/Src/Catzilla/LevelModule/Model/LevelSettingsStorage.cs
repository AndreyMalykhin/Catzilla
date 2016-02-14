using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelModule.Model {
    [CreateAssetMenuAttribute]
    public class LevelSettingsStorage: ScriptableObject {
        [Tooltip("In seconds")]
        [SerializeField]
        private int minLevelDuration = 60;

        [Tooltip("In seconds")]
        [SerializeField]
        private int levelDurationFactor = 10;

        [SerializeField]
        private int areaMinScoreableObjects = 4;

        [SerializeField]
        private int areaMaxScoreableObjects = 16;

        [SerializeField]
        private float areaScoreableObjectsLevelFactor = 2f;

        [SerializeField]
        private int areaMinDangerousObjects = 2;

        [SerializeField]
        private int areaMaxDangerousObjects = 8;

        [SerializeField]
        private float areaDangerousObjectsLevelFactor = 1f;

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
        private ScoreableView[] scoreableProtos;

        [Inject("LevelAreaDepth")]
        [NonSerialized]
        private float areaDepth;

        [Inject("LevelAreaWidth")]
        [NonSerialized]
        private float areaWidth;

        [NonSerialized]
        private readonly IDictionary<int, LevelSettings> items =
            new Dictionary<int, LevelSettings>();

        public LevelSettings Get(int levelIndex) {
            LevelSettings item;

            if (!items.TryGetValue(levelIndex, out item)) {
                item = MakeItem(levelIndex);
                items[levelIndex] = item;
            }

            return item;
        }

        private LevelSettings MakeItem(int levelIndex) {
            float objectAvgScore = 0f;

            for (int i = 0; i < scoreableProtos.Length; ++i) {
                objectAvgScore += scoreableProtos[i].Score;
            }

            objectAvgScore /= scoreableProtos.Length;
            float playerFrontSpeed = Mathf.Min(playerMinFrontSpeed +
                levelIndex * playerFrontSpeedLevelFactor, playerMaxFrontSpeed);
            float playerSideSpeed = Mathf.Min(playerMinSideSpeed +
                levelIndex * playerSideSpeedLevelFactor, playerMaxSideSpeed);
            float areaPassTime = areaDepth / playerFrontSpeed;
            float areaDangerousObjects = Mathf.Min(
                areaMinDangerousObjects + levelIndex *
                    areaDangerousObjectsLevelFactor,
                areaMaxDangerousObjects);
            float areaScoreableObjects = Mathf.Min(
                areaMinScoreableObjects + levelIndex *
                    areaScoreableObjectsLevelFactor,
                areaMaxScoreableObjects);
            float objectReachabilityPlayerFrontSpeedFactor =
                1f - playerFrontSpeed / areaDepth;
            float objectReachabilityPlayerSideSpeedFactor =
                Mathf.Log(playerSideSpeed) / Mathf.Log(areaWidth);
            float objectReachabilityDangerFactor = 1 - Mathf.Pow(
                areaDangerousObjects / (areaMaxDangerousObjects + 1), 2f);
            float objectReachability =
                (objectReachabilityPlayerFrontSpeedFactor +
                objectReachabilityPlayerSideSpeedFactor +
                objectReachabilityDangerFactor) / 3f;
            float areaAvgScore = objectReachability *
                areaScoreableObjects * objectAvgScore;
            float avgScorePerSec = areaAvgScore / areaPassTime;
            float completionScore = (minLevelDuration + levelIndex *
                levelDurationFactor) * avgScorePerSec;
            float resurrectionReward = resurrectionMinReward + levelIndex *
                resurrectionRewardLevelFactor;
            return new LevelSettings(
                levelIndex,
                (int) completionScore,
                (int) areaDangerousObjects,
                (int) areaScoreableObjects,
                playerFrontSpeed,
                playerSideSpeed,
                (int) resurrectionReward);
        }
    }
}
