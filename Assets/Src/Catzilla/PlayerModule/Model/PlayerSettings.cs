using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerSettings {
        public int Level {get; set;}
        public int ScoreRecord {get; set;}
        public int AvailableResurrectionsCount {get; set;}

        public PlayerSettings() {
            AvailableResurrectionsCount = 1;
        }
    }
}
