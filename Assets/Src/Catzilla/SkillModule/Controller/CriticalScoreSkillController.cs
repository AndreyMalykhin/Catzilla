using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class CriticalScoreSkillController {
        public void OnConstruct(Evt evt) {
            var criticalScoreSkill = (CriticalScoreSkillView) evt.Source;
            var player = criticalScoreSkill.GetComponent<PlayerView>();
            player.OnScoreFilter += criticalScoreSkill.OnScoreFilter;
        }
    }
}
