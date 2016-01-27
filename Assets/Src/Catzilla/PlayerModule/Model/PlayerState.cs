using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerState {
        public int Level {get; set;}
        public int ScoreRecord {get; set;}
        public int AvailableResurrectionsCount {get; set;}

        public PlayerState() {
            AvailableResurrectionsCount = 1;
        }
    }
}
