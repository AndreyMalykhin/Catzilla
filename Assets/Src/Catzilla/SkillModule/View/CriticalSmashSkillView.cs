using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.SkillModule.View {
    public class CriticalSmashSkillView: MonoBehaviour {
        public float Chance;
        public float Factor;

        [Inject]
        private EventBus eventBus;

        [PostInject]
        public void OnConstruct() {
            eventBus.Fire(
                (int) Events.CriticalSmashSkillConstruct, new Evt(this));
        }

        public CriticalValue OnScoreFilter(
            CriticalValue score, ScoreableView source = null) {
            // DebugUtils.Log("CriticalSmashSkillView.OnScoreFilter()");
            if (source != null
                && UnityEngine.Random.value <= Chance
                && source.GetComponent<SmashableView>() != null) {
                bool isCritical = true;
                return new CriticalValue(
                    Mathf.RoundToInt(score * Factor), isCritical);
            }

            return score;
        }
    }
}
