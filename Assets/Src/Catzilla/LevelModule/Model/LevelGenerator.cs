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
            Start,
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

        public int InitialAreasCount {get {return 2;}}
        public int ActiveBonusObjects {get; set;}

        private State nextState;
        private readonly IDictionary<State, StateTransition[]> states =
            new Dictionary<State, StateTransition[]>();

        [PostInject]
        public void OnConstruct() {
            states[State.Start] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.HoodMiddle)
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
            nextState = State.Start;
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(outputLevel.Index);
            ActiveBonusObjects = 0;
            PlayerView player = null;
            bool spawnPlayer = true;
            NewArea(
                levelSettings,
                spawnPlayer,
                outputLevel,
                player,
                (LevelAreaView area1) => {
                    spawnPlayer = false;
                    NewArea(
                        levelSettings,
                        spawnPlayer,
                        outputLevel,
                        player,
                        (LevelAreaView area2) => {
                            if (onDone != null) onDone();
                        });
                });
        }

        public void NewArea(PlayerView player, LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewArea()");
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(outputLevel.Index);
            bool spawnPlayer = false;
            NewArea(levelSettings, spawnPlayer, outputLevel, player, onDone);
        }

        private void NewArea(
            LevelSettings levelSettings,
            bool spawnPlayer,
            LevelView outputLevel,
            PlayerView player = null,
            Action<LevelAreaView> onDone = null) {
            EnvType envType = NextState(levelSettings);
            EnvTypeInfo envTypeInfo = EnvTypeInfoStorage.Get(envType);
            SpawnsInfo[] spawnsInfos =
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

        private SpawnsInfo[] GetSpawnsInfos(
            LevelSettings levelSettings,
            EnvTypeInfo envTypeInfo,
            bool spawnPlayer,
            PlayerView player = null) {
            LevelObjectType[] objectTypes =
                envTypeInfo.SpawnMap.GetObjectTypes();
            Array.Sort(objectTypes, ObjectTypeComparator);
            int objectTypesCount = objectTypes.Length;
            var spawnsInfos = new SpawnsInfo[objectTypesCount];

            for (int i = 0; i < objectTypesCount; ++i) {
                LevelObjectType objectType = objectTypes[i];
                ObjectTypeInfo objectTypeInfo =
                    ObjectTypeInfoStorage.Get(objectType);
                int spawnsCount = 0;

                if (UnityEngine.Random.value <= objectTypeInfo.SpawnChance) {
                    LevelObjectView objectProto = objectTypeInfo.ViewProto;
                    spawnsCount = objectTypeInfo.GetSpawnsPerArea(
                        levelSettings.Index);

                    if (objectTypeInfo.IsSpawnsCountRandom) {
                        spawnsCount = UnityEngine.Random.Range(
                            objectTypeInfo.MinSpawnsPerArea, spawnsCount + 1);
                    }

                    var bonus = objectProto.GetComponent<BonusView>();

                    if (objectType == PlayerObjectType) {
                        spawnsCount = spawnPlayer ? 1 : 0;
                        spawnPlayer = false;
                    } else if (bonus != null) {
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

                spawnsInfos[i] = new SpawnsInfo(objectType, spawnsCount);
            }

            return spawnsInfos;
        }

        private int ObjectTypeComparator(
            LevelObjectType lhs, LevelObjectType rhs) {
            return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
        }
    }
}
