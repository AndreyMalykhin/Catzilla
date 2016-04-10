using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class DamageAbsorptionSkillController {
        public void OnConstruct(Evt evt) {
            // DebugUtils.Log("DamageAbsorptionSkillController.OnConstruct()");
            var skill = (DamageAbsorptionSkillView) evt.Source;
            var player = skill.GetComponent<PlayerView>();
            player.OnAttackFilter += skill.OnAttackFilter;
        }
    }
}
