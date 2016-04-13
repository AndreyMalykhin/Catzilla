using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class PermanentSideSpeedBoostSkillHelper: ISkillHelper {
        [Inject]
        private IInstantiator instantiator;

        string ISkillHelper.GetSkillDescription(
            Skill skill, string description) {
            return string.Format(
                description, Mathf.RoundToInt(skill.Factor * 100f));
        }

        void ISkillHelper.AddSkill(Skill skill, GameObject obj) {
            // DebugUtils.Log("PermanentSideSpeedBoostSkillHelper.AddSkill()");
            var skillView = instantiator
                .InstantiateComponent<PermanentSideSpeedBoostSkillView>(obj);
            skillView.Init(obj.GetComponent<PlayerView>(), skill.Factor);
        }
    }
}
