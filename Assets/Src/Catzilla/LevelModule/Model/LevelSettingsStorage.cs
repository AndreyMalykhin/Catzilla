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
        [Tooltip("In seconds")]
        [SerializeField]
        private int minLevelDuration = 60;

        [Tooltip("In seconds")]
        [SerializeField]
        private int levelDurationFactor = 10;

        [SerializeField]
        private int areaAvgDangerousObjectTypes = 2;

        [SerializeField]
        private int areaAvgScoreableObjectTypes = 3;

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

        [Inject("LevelAreaDepth")]
        [NonSerialized]
        private float areaDepth;

        [Inject("LevelAreaWidth")]
        [NonSerialized]
        private float areaWidth;

        [Inject]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

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
            float areaAvgDangerousObjects = 0;
            float areaAvgScoreableObjects = 0;
            var objectTypeInfos = objectTypeInfoStorage.GetAll();
            var objectScores = new List<int>(objectTypeInfos.Count);

            foreach (ObjectTypeInfo objectTypeInfo in objectTypeInfos) {
                LevelObjectView objectProto = objectTypeInfo.ViewProto;

                if (objectProto.GetComponent<DangerousView>() != null) {
                    areaAvgDangerousObjects +=
                        objectTypeInfo.GetSpawnsPerArea(levelIndex);
                }

                if (objectProto.GetComponent<ScoreableView>() != null) {
                    objectScores.Add(
                        objectProto.GetComponent<ScoreableView>().Score);
                    areaAvgScoreableObjects +=
                        objectTypeInfo.GetSpawnsPerArea(levelIndex);
                }
            }

            areaAvgDangerousObjects *=
                (float) areaAvgDangerousObjectTypes / objectTypeInfos.Count;
            areaAvgScoreableObjects *=
                (float) areaAvgScoreableObjectTypes / objectTypeInfos.Count;
            float playerFrontSpeed = Mathf.Min(playerMinFrontSpeed +
                levelIndex * playerFrontSpeedLevelFactor, playerMaxFrontSpeed);
            float playerSideSpeed = Mathf.Min(playerMinSideSpeed +
                levelIndex * playerSideSpeedLevelFactor, playerMaxSideSpeed);
            float areaPassTime = areaDepth / playerFrontSpeed;
            float objectReachabilityPlayerFrontSpeedFactor =
                1f - Mathf.Log(playerFrontSpeed) / Mathf.Log(areaDepth + 1);
            float objectReachabilityPlayerSideSpeedFactor =
                playerSideSpeed / areaWidth;
            float objectReachabilityDangerFactor =
                1 - areaAvgDangerousObjects / (areaAvgScoreableObjects + 1);
            float objectReachability =
                (objectReachabilityPlayerFrontSpeedFactor +
                objectReachabilityPlayerSideSpeedFactor +
                objectReachabilityDangerFactor) / 3f;
            objectScores.Sort();
            float objectAvgScore =
                objectScores[Mathf.CeilToInt(0.75f * objectScores.Count) - 1];
            float areaAvgScore = objectReachability *
                areaAvgScoreableObjects * objectAvgScore;
            float avgScorePerSec = areaAvgScore / areaPassTime;
            float completionScore = (minLevelDuration + levelIndex *
                levelDurationFactor) * avgScorePerSec;
            float resurrectionReward = resurrectionMinReward + levelIndex *
                resurrectionRewardLevelFactor;
            // DebugUtils.Log("LevelSettingsStorage.MakeItem(); objectReachabilityPlayerFrontSpeedFactor={0}; objectReachabilityPlayerSideSpeedFactor={1}; objectReachabilityDangerFactor={2}; objectReachability={3}; objectAvgScore={4}", objectReachabilityPlayerFrontSpeedFactor, objectReachabilityPlayerSideSpeedFactor, objectReachabilityDangerFactor, objectReachability, objectAvgScore);
            return new LevelSettings(
                levelIndex,
                (int) completionScore,
                playerFrontSpeed,
                playerSideSpeed,
                (int) resurrectionReward);
        }
    }
}
