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

        [Inject]
        public LevelAreaGenerator AreaGenerator {get; set;}

        [Inject]
        public EnvTypeInfoStorage EnvTypeInfoStorage {get; set;}

        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject("PlayerObjectType")]
        public LevelObjectType PlayerObjectType {get; set;}

        public int InitialAreasCount {get {return 3;}}
        public int ActiveBonusObjects {get; set;}

        private State nextState;
        private readonly IDictionary<State, StateTransition[]> states =
            new Dictionary<State, StateTransition[]>();
        private readonly List<SpawnsInfo> spawnsInfosBuffer =
            new List<SpawnsInfo>(16);

        [PostInject]
        public void OnConstruct() {
            states[State.First] = new StateTransition[] {
                new StateTransition(EnvType.HoodEnd, State.Second)
            };
            states[State.Second] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.Third)
            };
            states[State.Third] = new StateTransition[] {
                new StateTransition(EnvType.HoodMiddle, State.HoodMiddle)
            };
            states[State.TrackMiddle] = new StateTransition[] {
                new StateTransition(EnvType.TrackMiddle, State.TrackMiddle),
                new StateTransition(EnvType.TrackEnd, State.TrackEnd)
            };
            states[State.TrackEnd] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.HoodMiddle),
                new StateTransition(EnvType.ParkStart, State.ParkMiddle)
            };
            states[State.HoodMiddle] = new StateTransition[] {
                new StateTransition(EnvType.HoodMiddle, State.HoodMiddle),
                new StateTransition(EnvType.HoodEnd, State.HoodEnd)
            };
            states[State.HoodEnd] = new StateTransition[] {
                new StateTransition(EnvType.TrackStart, State.TrackMiddle),
                new StateTransition(EnvType.ParkStart, State.ParkMiddle)
            };
            states[State.ParkMiddle] = new StateTransition[] {
                new StateTransition(EnvType.ParkMiddle, State.ParkMiddle),
                new StateTransition(EnvType.ParkEnd, State.ParkEnd)
            };
            states[State.ParkEnd] = new StateTransition[] {
                new StateTransition(EnvType.TrackStart, State.TrackMiddle),
                new StateTransition(EnvType.HoodStart, State.HoodMiddle)
            };
        }

        public void NewLevel(
            int levelIndex, LevelView outputLevel, Action onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewLevel()");
            outputLevel.Init(levelIndex);
            nextState = State.First;
            ActiveBonusObjects = 0;
            spawnsInfosBuffer.Clear();
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(outputLevel.Index);
            EnvType envType = NextState(levelSettings);
            EnvTypeInfo envTypeInfo = EnvTypeInfoStorage.Get(envType);
            AreaGenerator.NewArea(
                envTypeInfo,
                spawnsInfosBuffer,
                levelSettings,
                outputLevel,
                (LevelAreaView area1) => {
                    envType = NextState(levelSettings);
                    envTypeInfo = EnvTypeInfoStorage.Get(envType);
                    PlayerView player = null;
                    bool spawnPlayer = true;
                    List<SpawnsInfo> spawnsInfos = GetSpawnsInfos(
                        levelSettings,
                        envTypeInfo,
                        spawnPlayer,
                        player);
                    AreaGenerator.NewArea(
                        envTypeInfo,
                        spawnsInfos,
                        levelSettings,
                        outputLevel,
                        (LevelAreaView area2) => {
                            envType = NextState(levelSettings);
                            envTypeInfo = EnvTypeInfoStorage.Get(envType);
                            spawnPlayer = false;
                            spawnsInfos = GetSpawnsInfos(
                                levelSettings,
                                envTypeInfo,
                                spawnPlayer,
                                player);
                            AreaGenerator.NewArea(
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
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(outputLevel.Index);
            bool spawnPlayer = false;
            EnvType envType = NextState(levelSettings);
            EnvTypeInfo envTypeInfo = EnvTypeInfoStorage.Get(envType);
            List<SpawnsInfo> spawnsInfos =
                GetSpawnsInfos(levelSettings, envTypeInfo, spawnPlayer, player);
            AreaGenerator.NewArea(
                envTypeInfo,
                spawnsInfos,
                levelSettings,
                outputLevel,
                onDone);
        }

        private EnvType NextState(LevelSettings levelSettings) {
            StateTransition[] stateTransitions = states[nextState];
            int stateTransitionsCount = stateTransitions.Length;
            int weightsSum = 0;
            int levelIndex = levelSettings.Index;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                EnvTypeInfo envTypeInfo =
                    EnvTypeInfoStorage.Get(stateTransitions[i].EnvType);
                weightsSum += envTypeInfo.GetSpawnWeight(levelIndex);
            }

            int randomWeight = UnityEngine.Random.Range(1, weightsSum + 1);
            int currentWeight = 0;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                var stateTransition = stateTransitions[i];
                EnvTypeInfo envTypeInfo =
                    EnvTypeInfoStorage.Get(stateTransition.EnvType);
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
            LevelObjectType[] objectTypes =
                envTypeInfo.SpawnMap.GetObjectTypes();
            Array.Sort(objectTypes, ObjectTypeComparator);
            spawnsInfosBuffer.Clear();
            int objectTypesCount = objectTypes.Length;

            for (int i = 0; i < objectTypesCount; ++i) {
                LevelObjectType objectType = objectTypes[i];
                ObjectTypeInfo objectTypeInfo =
                    ObjectTypeInfoStorage.Get(objectType);
                ObjectLevelSettings objectLevelSettings =
                    levelSettings.GetObjectSettings(objectType);
                int spawnsCount = 0;

                if (objectType == PlayerObjectType) {
                    spawnsCount = spawnPlayer ? 1 : 0;
                    spawnPlayer = false;
                } else if (objectLevelSettings != null
                           && UnityEngine.Random.value <=
                               objectTypeInfo.SpawnChance) {
                    LevelObjectView objectProto = objectTypeInfo.ProtoInfo.View;
                    spawnsCount = UnityEngine.Random.Range(
                        objectLevelSettings.MinSpawnsPerArea,
                        objectLevelSettings.MaxSpawnsPerArea + 1);
                    var bonus = objectProto.GetComponent<BonusView>();

                    if (bonus != null) {
                        if (player == null
                            || ActiveBonusObjects > 0
                            || !bonus.IsNeeded(player, levelSettings)
                            || !bonus.CanGive(player, levelSettings)) {
                            spawnsCount = 0;
                        } else {
                            ActiveBonusObjects += spawnsCount;
                        }
                    }
                }

                spawnsInfosBuffer.Add(new SpawnsInfo(objectType, spawnsCount));
            }

            return spawnsInfosBuffer;
        }

        private int ObjectTypeComparator(
            LevelObjectType lhs, LevelObjectType rhs) {
            return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
        }
    }
}
