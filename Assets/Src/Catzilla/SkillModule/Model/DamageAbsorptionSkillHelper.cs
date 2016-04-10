using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class DamageAbsorptionSkillHelper: ISkillHelper {
        [Inject]
        private IInstantiator instantiator;

        string ISkillHelper.GetSkillDescription(
            Skill skill, string description) {
            return string.Format(
                description,
                Mathf.RoundToInt(skill.Chance * 100f),
                Mathf.RoundToInt(skill.Factor * 100f));
        }

        void ISkillHelper.AddSkill(Skill skill, GameObject obj) {
            // DebugUtils.Log("DamageAbsorptionSkillHelper.AddSkill()");
            var skillView = instantiator
                .InstantiateComponent<DamageAbsorptionSkillView>(obj);
            skillView.Chance = skill.Chance;
            skillView.Factor = skill.Factor;
        }
    }
}
