using UnityEngine;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.SkillModule.View {
    public class CriticalScoreSkillView: MonoBehaviour {
        public float Chance;
        public float Factor;

        public CriticalValue OnScoreFilter(
            CriticalValue score, ScoreableView source = null) {
            if (source != null && UnityEngine.Random.value <= Chance) {
                bool isCritical = true;
                return new CriticalValue((int) (score * Factor), isCritical);
            }

            return score;
        }
    }
}
