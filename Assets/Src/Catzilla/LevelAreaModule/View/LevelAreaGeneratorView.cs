using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.SkillModule.Model;
using Catzilla.SkillModule.View;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaGeneratorView: MonoBehaviour {
        [Inject]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

        [Inject("PlayerObjectType")]
        private LevelObjectType playerObjectType;

        [SerializeField]
        [Tooltip("In seconds")]
        private float objectSpawnRate;

        [Inject]
        private SkillStorage skillStorage;

        [Inject]
        private SkillHelperStorage skillHelperStorage;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        private readonly IDictionary<Vector3, bool> reservedSpawnPoints =
            new Dictionary<Vector3, bool>(64, new Vector3Comparer());
        private LevelAreaView area;
        private EnvTypeInfo envTypeInfo;
        private List<SpawnsInfo> spawnsInfos;
        private LevelSettings levelSettings;
        private LevelView level;
        private int spawnsInfosIndex;
        private int objectsCountToSpawn;
        private int spawnTriesCount;
        private Action<LevelAreaView> onDone;

        public void NewArea(
            EnvTypeInfo envTypeInfo,
            List<SpawnsInfo> spawnsInfos,
            LevelSettings levelSettings,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log(
            //     "LevelAreaGeneratorView.NewArea(); envType={0}", envType);
            area = outputLevel.NewArea(envTypeInfo);

            if (spawnsInfos.Count == 0) {
                if (onDone != null) onDone(area);
                return;
            }

            Profiler.BeginSample("LevelAreaGeneratorView.NewArea()");
            this.envTypeInfo = envTypeInfo;
            this.spawnsInfos = spawnsInfos;
            this.levelSettings = levelSettings;
            this.onDone = onDone;
            level = outputLevel;
            spawnsInfosIndex = 0;
            NextObjectType();
            reservedSpawnPoints.Clear();
            InvokeRepeating("NewObject", objectSpawnRate, objectSpawnRate);
            Profiler.EndSample();
        }

        public void Stop() {
            if (!IsInvoking("NewObject")) {
                return;
            }

            CancelInvoke("NewObject");
        }

        private void NewObject() {
            // DebugUtils.Log("LevelAreaGeneratorView.NewObject()");
            Profiler.BeginSample("LevelAreaGeneratorView.NewObject()");
            LevelObjectType objectType =
                spawnsInfos[spawnsInfosIndex].ObjectType;
            List<SpawnLocation> spawnLocations =
                envTypeInfo.SpawnMap.GetLocations(objectType);
            ObjectTypeInfo objectTypeInfo =
                objectTypeInfoStorage.Get(objectType);
            Vector3 spawnPoint;
            SpawnLocation spawnLocation;
            bool isFreeSpaceExists = true;

            do {
                spawnLocation = spawnLocations[
                    UnityEngine.Random.Range(0, spawnLocations.Count)];
                spawnPoint = GetRandomSpawnPoint(
                    spawnLocation.Bounds, objectTypeInfo);

                if (spawnTriesCount == 0) {
                    isFreeSpaceExists = false;
                    break;
                }

                --spawnTriesCount;
            } while (IsSpawnPointReserved(spawnPoint));

            if (isFreeSpaceExists) {
                int objectProtoIndex = UnityEngine.Random.Range(
                    0, objectTypeInfo.ProtoInfos.Length);
                ObjectProtoInfo objectProtoInfo =
                    objectTypeInfo.ProtoInfos[objectProtoIndex];
                LevelObjectView obj = level.NewObject(
                    objectProtoInfo.View, spawnPoint, area.Index);
                InitObject(obj, objectTypeInfo, objectProtoInfo, spawnLocation,
                    levelSettings);
                ReserveSpawnPoint(spawnPoint);
            }

            --objectsCountToSpawn;
            Profiler.EndSample();

            if (objectsCountToSpawn <= 0 || spawnTriesCount <= 0) {
                ++spawnsInfosIndex;

                if (spawnsInfosIndex >= spawnsInfos.Count) {
                    CancelInvoke("NewObject");
                    if (onDone != null) onDone(area);
                    return;
                }

                NextObjectType();
            }
        }

        private void NextObjectType() {
            objectsCountToSpawn = spawnsInfos[spawnsInfosIndex].Count;
            spawnTriesCount = objectsCountToSpawn * 2;
        }

        private Vector3 GetRandomSpawnPoint(
            Bounds spawnLocation, ObjectTypeInfo objectTypeInfo) {
            Vector3 spawnLocationSize = spawnLocation.size;
            int objectWidth = objectTypeInfo.Width;
            int objectDepth = objectTypeInfo.Depth;
            DebugUtils.Assert(spawnLocationSize.x >= objectWidth);
            DebugUtils.Assert(spawnLocationSize.z >= objectDepth);
            int x = UnityEngine.Random.Range(
                0,
                Mathf.RoundToInt(spawnLocationSize.x) / objectWidth);
            int z = UnityEngine.Random.Range(
                0,
                Mathf.RoundToInt(spawnLocationSize.z) / objectDepth);
            Vector3 spawnLocationStart = spawnLocation.min;
            return new Vector3(
                spawnLocationStart.x + objectWidth / 2f + x * objectWidth,
                spawnLocation.center.y,
                spawnLocationStart.z + objectDepth / 2f + z * objectDepth);
        }

        private bool IsSpawnPointReserved(Vector3 point) {
            return reservedSpawnPoints.ContainsKey(point);
        }

        private void ReserveSpawnPoint(Vector3 point) {
            reservedSpawnPoints[point] = true;
        }

        private void InitObject(
            LevelObjectView obj,
            ObjectTypeInfo objectTypeInfo,
            ObjectProtoInfo objectProtoInfo,
            SpawnLocation spawnLocation,
            LevelSettings levelSettings) {
            obj.transform.rotation =
                Quaternion.Euler(0f, spawnLocation.IsXFlipped ? 180f : 0f, 0f);
            var alternating = obj.GetComponent<AlternatingView>();

            if (alternating != null) {
                int materialIndex = UnityEngine.Random.Range(
                    0, objectProtoInfo.AvailableMaterials.Length);
                alternating.Material =
                    objectProtoInfo.AvailableMaterials[materialIndex];
            }

            if (objectTypeInfo.Type == playerObjectType) {
                var player = obj.GetComponent<PlayerView>();
                player.FrontSpeed = levelSettings.PlayerFrontSpeed;
                player.SideSpeed = levelSettings.PlayerSideSpeed;
                List<int> skillIds = playerStateStorage.Get().SkillIds;

                for (int i = 0; i < skillIds.Count; ++i) {
                    Skill skill = skillStorage.Get(skillIds[i]);
                    BaseSkill baseSkill = skillStorage.GetBase(skill.BaseId);
                    ISkillHelper skillHelper =
                        skillHelperStorage.Get(baseSkill.Type);
                    skillHelper.AddSkill(skill, obj.gameObject);
                }
            }
        }
    }
}
