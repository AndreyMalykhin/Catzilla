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
        private enum State {Start, TrackMiddle, HoodMiddle, ParkMiddle}

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

        public int InitialAreasCount {get {return 2;}}

        private State nextState;
        private readonly IDictionary<State, StateTransition[]> states =
            new Dictionary<State, StateTransition[]>();

        [PostInject]
        public void OnConstruct() {
            states[State.Start] = new StateTransition[] {
                new StateTransition(EnvType.HoodStart, State.HoodMiddle),
                new StateTransition(EnvType.ParkStart, State.ParkMiddle),
                new StateTransition(EnvType.TrackStart, State.TrackMiddle)
            };
            states[State.TrackMiddle] = new StateTransition[] {
                new StateTransition(EnvType.TrackMiddle, State.TrackMiddle),
                new StateTransition(EnvType.TrackEnd, State.Start)
            };
            states[State.HoodMiddle] = new StateTransition[] {
                new StateTransition(EnvType.HoodMiddle, State.HoodMiddle),
                new StateTransition(EnvType.HoodEnd, State.Start)
            };
            states[State.ParkMiddle] = new StateTransition[] {
                new StateTransition(EnvType.ParkMiddle, State.ParkMiddle),
                new StateTransition(EnvType.ParkEnd, State.Start)
            };
        }

        public void NewLevel(
            int levelIndex, LevelView outputLevel, Action onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewLevel()");
            outputLevel.Init(levelIndex);
            nextState = State.Start;
            bool spawnPlayer = true;
            NewArea(spawnPlayer, outputLevel, (area1) => {
                    NewArea(false, outputLevel, (area2) => {
                    if (onDone != null) onDone();
                });
            });
        }

        public void NewArea(
            LevelView outputLevel, Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log("LevelGenerator.NewArea()");
            bool spawnPlayer = false;
            NewArea(spawnPlayer, outputLevel, onDone);
        }

        private void NewArea(
            bool spawnPlayer,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            AreaGenerator.NewArea(
                NextState(),
                spawnPlayer,
                outputLevel,
                onDone);
        }

        private EnvType NextState() {
            StateTransition[] stateTransitions = states[nextState];
            int stateTransitionsCount = stateTransitions.Length;
            int weightsSum = 0;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                weightsSum += EnvTypeInfoStorage.Get(
                    stateTransitions[i].EnvType).SpawnWeight;
            }

            int randomWeight = UnityEngine.Random.Range(1, weightsSum + 1);
            int weightIntervalEnd = 0;

            for (int i = 0; i < stateTransitionsCount; ++i) {
                var stateTransition = stateTransitions[i];
                weightIntervalEnd += EnvTypeInfoStorage.Get(
                    stateTransition.EnvType).SpawnWeight;

                if (weightIntervalEnd >= randomWeight) {
                    nextState = stateTransition.NextState;
                    return stateTransition.EnvType;
                }
            }

            DebugUtils.Assert(false);
            return 0;
        }
    }
}
