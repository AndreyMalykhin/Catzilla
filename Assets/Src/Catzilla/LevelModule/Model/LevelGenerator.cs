using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Model {
    public class LevelGenerator {
        [Inject]
        public LevelAreaGenerator AreaGenerator {get; set;}

        private delegate EnvType State();

        private State state;

        public void NewLevel(LevelView level) {
            Debug.Log("LevelGenerator.NewLevel()");
            state = StateStart;
            bool spawnPlayer = true;
            NewArea(spawnPlayer, level);
        }

        public void NewArea(LevelView level) {
            Debug.Log("LevelGenerator.NewArea()");
            bool spawnPlayer = false;
            NewArea(spawnPlayer, level);
        }

        private void NewArea(bool spawnPlayer, LevelView level) {
            AreaGenerator.NewArea(state(), spawnPlayer, level);
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
