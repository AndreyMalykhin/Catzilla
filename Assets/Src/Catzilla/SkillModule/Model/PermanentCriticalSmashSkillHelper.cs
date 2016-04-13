using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class PermanentCriticalSmashSkillHelper: ISkillHelper {
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
            // DebugUtils.Log("PermanentCriticalSmashSkillHelper.AddSkill()");
            var skillView = instantiator
                .InstantiateComponent<PermanentCriticalSmashSkillView>(obj);
            skillView.Init(
                obj.GetComponent<PlayerView>(), skill.Chance, skill.Factor);
        }
    }
}
