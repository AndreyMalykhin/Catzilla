using UnityEngine;
using System.Collections;

namespace Catzilla.LevelModule.Model {
    [CreateAssetMenuAttribute]
    public class LevelSettingsStorage: ScriptableObject {
        [SerializeField]
        private int baseCompletionScore = 1;

        [SerializeField]
        private float completionScoreFactor = 2f;

        public LevelSettings Get(int levelIndex) {
            int completionScore = baseCompletionScore +
                (int) (levelIndex * completionScoreFactor);
            return new LevelSettings(levelIndex, completionScore);
        }
    }
}
