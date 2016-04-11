using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class CriticalSmashSkillController {
        private CriticalSmashSkillView skill;

        public void OnConstruct(Evt evt) {
            // DebugUtils.Log("CriticalSmashSkillController.OnConstruct()");
            skill = (CriticalSmashSkillView) evt.Source;
        }

        public void OnSmashStreak(Evt evt) {
            if (skill == null) {
                return;
            }

            skill.Trigger();
        }
    }
}
