using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    public struct LevelSettings {
        public int Index {get; private set;}
        public int CompletionScore {get; private set;}

        public LevelSettings(int index, int completionScore) {
            Index = index;
            CompletionScore = completionScore;
        }
    }
}
