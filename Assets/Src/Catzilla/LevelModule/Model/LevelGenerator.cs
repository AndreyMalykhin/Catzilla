using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    public class LevelGenerator {
        [Inject]
        public LevelAreaGenerator AreaGenerator {get; set;}

        private delegate EnvType State();

        private State state;

        public Level Generate(int levelIndex) {
            state = StateStart;
            var areas = new List<LevelArea>(1);
            bool spawnPlayer = true;
            areas.Add(GenerateArea(levelIndex, spawnPlayer));
            return new Level(levelIndex, areas);
        }

        public LevelArea GenerateArea(int levelIndex) {
            bool spawnPlayer = false;
            return GenerateArea(levelIndex, spawnPlayer);
        }

        private LevelArea GenerateArea(int levelIndex, bool spawnPlayer) {
            return AreaGenerator.Generate(state(), levelIndex, spawnPlayer);
        }

        private EnvType StateStart() {
            switch (Random.Range(0, 3)) {
                case 0:
                    state = StateHoodMiddle;
                    return EnvType.HoodStart;
                case 1:
                    state = StateParkMiddle;
                    return EnvType.ParkStart;
                default:
                    state = StateTrackMiddle;
                    return EnvType.TrackStart;
            }
        }

        private EnvType StateTrackMiddle() {
            switch (Random.Range(0, 4)) {
                case 0:
                    state = StateStart;
                    return EnvType.TrackEnd;
                default:
                    state = StateTrackMiddle;
                    return EnvType.TrackMiddle;
            }
        }

        private EnvType StateHoodMiddle() {
            switch (Random.Range(0, 3)) {
                case 0:
                    state = StateStart;
                    return EnvType.HoodEnd;
                default:
                    state = StateHoodMiddle;
                    return EnvType.HoodMiddle;
            }
        }

        private EnvType StateParkMiddle() {
            switch (Random.Range(0, 2)) {
                case 0:
                    state = StateStart;
                    return EnvType.ParkEnd;
                default:
                    state = StateParkMiddle;
                    return EnvType.ParkMiddle;
            }
        }
    }
}
