using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Model {
    public class LevelGenerator {
        private enum State {
            First,
            Second,
            Third,
            TrackMiddle,
            TrackEnd,
            HoodMiddle,
            HoodEnd,
            ParkMiddle,
            ParkEnd
        }

        private struct StateTransition {
            public readonly EnvType EnvType;
            public readonly State NextState;

            public StateTransition(EnvType envType, State nextState) {
                EnvType = envType;
                NextState = nextState;
            }
        }

        public int InitialAreasCount {get {return 3;}}

        [Inject]
        private LevelAreaGenerator areaGenerator;

        [Inject]
        private EnvTypeInfoStorage envTypeInfoStorage;

        [Inject]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

        [Inject]
        private LevelSettingsStorage levelSettingsStorage;

        [Inject("PlayerObjectType")]
        private LevelObjectType playerObjectType;

        [Inject]
        private BonusSpawnResolver bonusSpawnResolver;

        private State nextState;
        private readonly IDictionary<int, StateTransition[]> states =
            new Dictionary<int, StateTransition[]>(16);
        private readonly List<SpawnsInfo> spawnsInfosBuffer =
            new List<SpawnsInfo>(16);

        [PostInject]
        public void OnConstruct() {
            states[(int) State.First] = new StateTransition[] {
                new StateTransition(EnvType.HoodEnd, State.Second)
            };
            states[(int) State.Second] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.Third)
            };
            states[(int) State.Third] = new StateTransition[] {
                new StateTransition(EnvType.HoodMiddle, State.HoodMiddle)
            };
            states[(int) State.TrackMiddle] = new StateTransition[] {
                new StateTransition(EnvType.TrackMiddle, State.TrackMiddle),
                new StateTransition(EnvType.TrackEnd, State.TrackEnd)
            };
            states[(int) State.TrackEnd] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.HoodMiddle),
                new StateTransition(EnvType.ParkStart, State.ParkMiddle)
            };
            states[(int) State.HoodMiddle] = new StateTransition[] {
                new StateTransition(EnvType.HoodMiddle, State.HoodMiddle),
                new StateTransition(EnvType.HoodEnd, State.HoodEnd)
            };
            states[(int) State.HoodEnd] = new StateTransition[] {
                new StateTransition(EnvType.TrackStart, State.TrackMiddle),
                new StateTransition(EnvType.ParkStart, State.ParkMiddle)
            };
            states[(int) State.ParkMiddle] = new StateTransition[] {
                new StateTransition(EnvType.ParkMiddle, State.ParkMiddle),
                new StateTransition(EnvType.ParkEnd, State.ParkEnd)
            };
            states[(int) State.ParkEnd] = new StateTransition[] {
                new StateTransition(EnvType.TrackStart, State.TrackMiddle),
                new StateTransition(EnvType.HoodStart, State.HoodMiddle)
            };
        }

        public void NewLevel(
            int levelIndex, LevelView outputLevel, Action onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewLevel()");
            outputLevel.Init(levelIndex);
            nextState = State.First;
            bonusSpawnResolver.ActiveBonusObjects = 0;
            spawnsInfosBuffer.Clear();
            LevelSettings levelSettings =
                levelSettingsStorage.Get(outputLevel.Index);
            EnvType envType = NextState(levelSettings);
            EnvTypeInfo envTypeInfo = envTypeInfoStorage.Get(envType);
            areaGenerator.NewArea(
                envTypeInfo,
                spawnsInfosBuffer,
                levelSettings,
                outputLevel,
                (LevelAreaView area1) => {
                    envType = NextState(levelSettings);
                    envTypeInfo = envTypeInfoStorage.Get(envType);
                    PlayerView player = null;
                    bool spawnPlayer = true;
                    List<SpawnsInfo> spawnsInfos = GetSpawnsInfos(
                        levelSettings,
                        envTypeInfo,
                        spawnPlayer,
                        player);
                    areaGenerator.NewArea(
                        envTypeInfo,
                        spawnsInfos,
                        levelSettings,
                        outputLevel,
                        (LevelAreaView area2) => {
                            envType = NextState(levelSettings);
                            envTypeInfo = envTypeInfoStorage.Get(envType);
                            spawnPlayer = false;
                            spawnsInfos = GetSpawnsInfos(
                                levelSettings,
                                envTypeInfo,
                                spawnPlayer,
                                player);
                            areaGenerator.NewArea(
                                envTypeInfo,
                                spawnsInfos,
                                levelSettings,
                                outputLevel,
                                (LevelAreaView area3) => {
                                    if (onDone != null) onDone();
                                });
                        });
                });
        }

        public void NewArea(PlayerView player, LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewArea()");
            Profiler.BeginSample("LevelGenerator.NewArea()");
            LevelSettings levelSettings =
                levelSettingsStorage.Get(outputLevel.Index);
            bool spawnPlayer = false;
            EnvType envType = NextState(levelSettings);
            EnvTypeInfo envTypeInfo = envTypeInfoStorage.Get(envType);
            List<SpawnsInfo> spawnsInfos =
                GetSpawnsInfos(levelSettings, envTypeInfo, spawnPlayer, player);
            areaGenerator.NewArea(
                envTypeInfo,
                spawnsInfos,
                levelSettings,
                outputLevel,
                onDone);
            Profiler.EndSample();
        }

        private EnvType NextState(LevelSettings levelSettings) {
            StateTransition[] stateTransitions = states[(int) nextState];
            int stateTransitionsCount = stateTransitions.Length;
            int weightsSum = 0;
            int levelIndex = levelSettings.Index;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                EnvTypeInfo envTypeInfo =
                    envTypeInfoStorage.Get(stateTransitions[i].EnvType);
                weightsSum += envTypeInfo.GetSpawnWeight(levelIndex);
            }

            int randomWeight = UnityEngine.Random.Range(1, weightsSum + 1);
            int currentWeight = 0;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                var stateTransition = stateTransitions[i];
                EnvTypeInfo envTypeInfo =
                    envTypeInfoStorage.Get(stateTransition.EnvType);
                currentWeight += envTypeInfo.GetSpawnWeight(levelIndex);

                if (currentWeight >= randomWeight) {
                    nextState = stateTransition.NextState;
                    return stateTransition.EnvType;
                }
            }

            DebugUtils.Assert(false);
            return 0;
        }

        private List<SpawnsInfo> GetSpawnsInfos(
            LevelSettings levelSettings,
            EnvTypeInfo envTypeInfo,
            bool spawnPlayer,
            PlayerView player = null) {
            Profiler.BeginSample("LevelGenerator.GetSpawnsInfos()");
            LevelObjectType[] objectTypes =
                envTypeInfo.SpawnMap.ObjectTypes;
            Array.Sort(objectTypes, ObjectTypeComparator);
            spawnsInfosBuffer.Clear();
            int objectTypesCount = objectTypes.Length;

            for (int i = 0; i < objectTypesCount; ++i) {
                LevelObjectType objectType = objectTypes[i];
                ObjectTypeInfo objectTypeInfo =
                    objectTypeInfoStorage.Get(objectType);
                ObjectLevelSettings objectLevelSettings =
                    levelSettings.GetObjectSettings(objectType);
                int spawnsCount = 0;

                if (objectType == playerObjectType) {
                    spawnsCount = spawnPlayer ? 1 : 0;
                    spawnPlayer = false;
                } else if (objectLevelSettings != null
                           && UnityEngine.Random.value <=
                               objectLevelSettings.SpawnChance) {
                    LevelObjectView objectProto = objectTypeInfo.ProtoInfo.View;
                    spawnsCount = UnityEngine.Random.Range(
                        objectLevelSettings.MinSpawnsPerArea,
                        objectLevelSettings.MaxSpawnsPerArea + 1);
                    var bonus = objectProto.GetComponent<BonusView>();

                    if (bonus != null) {
                        if (player == null
                            || !bonusSpawnResolver.IsTimeToSpawn(
                                    bonus, player)) {
                            spawnsCount = 0;
                        } else {
                            bonusSpawnResolver.ActiveBonusObjects +=
                                spawnsCount;
                        }
                    }
                }

                spawnsInfosBuffer.Add(new SpawnsInfo(objectType, spawnsCount));
            }

            Profiler.EndSample();
            return spawnsInfosBuffer;
        }

        private int ObjectTypeComparator(
            LevelObjectType lhs, LevelObjectType rhs) {
            int result = objectTypeInfoStorage.Get(lhs).SpawnPriority
                - objectTypeInfoStorage.Get(rhs).SpawnPriority;

            if (result == 0) {
                return UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
            }

            return result;
        }
    }
}
