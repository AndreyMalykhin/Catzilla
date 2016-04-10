using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class CriticalSmashSkillController {
        public void OnConstruct(Evt evt) {
            // DebugUtils.Log("CriticalSmashSkillController.OnConstruct()");
            var skill = (CriticalSmashSkillView) evt.Source;
            var player = skill.GetComponent<PlayerView>();
            player.OnScoreFilter += skill.OnScoreFilter;
        }
    }
}
