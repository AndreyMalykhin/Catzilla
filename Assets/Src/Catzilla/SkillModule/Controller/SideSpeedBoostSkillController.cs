using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class SideSpeedBoostSkillController {
        private SideSpeedBoostSkillView skill;

        public void OnConstruct(Evt evt) {
            // DebugUtils.Log("SideSpeedBoostSkillController.OnConstruct()");
            skill = (SideSpeedBoostSkillView) evt.Source;
        }

        public void OnSmashStreak(Evt evt) {
            if (skill == null) {
                return;
            }

            skill.Trigger();
        }
    }
}
