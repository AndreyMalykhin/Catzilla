using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class SideSpeedBoostSkillHelper: ISkillHelper {
        [Inject]
        private IInstantiator instantiator;

        string ISkillHelper.GetSkillDescription(
            Skill skill, string description) {
            return string.Format(
                description,
                Mathf.RoundToInt(skill.Chance * 100f),
                Mathf.RoundToInt(skill.Factor * 100f),
                Mathf.RoundToInt(skill.Duration));
        }

        void ISkillHelper.AddSkill(Skill skill, GameObject obj) {
            // DebugUtils.Log("SideSpeedBoostSkillHelper.AddSkill()");
            var skillView = instantiator
                .InstantiateComponent<SideSpeedBoostSkillView>(obj);
            skillView.Init(obj.GetComponent<PlayerView>());
            skillView.Chance = skill.Chance;
            skillView.Factor = skill.Factor;
            skillView.Duration = skill.Duration;
        }
    }
}
