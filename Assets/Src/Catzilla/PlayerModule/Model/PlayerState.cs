using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    // TODO
    public class PlayerState {
        // public int Level {get; set;}
        public int Level {get {return 4;} set {}}
        public int ScoreRecord {get; set;}
        public int AvailableResurrectionsCount {get; set;}
    }
}
