using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class CriticalScoreSkillHelper: ISkillHelper {
        [Inject]
        private Translator translator;

        string ISkillHelper.GetSkillDescription(
            Skill skill, string description) {
            return translator.Translate(
                description, skill.Chance, skill.Factor);
        }

        void ISkillHelper.AddSkill(Skill skill, GameObject obj) {
            var skillView = obj.AddComponent<CriticalScoreSkillView>();
            skillView.Chance = skill.Chance;
            skillView.Factor = skill.Factor;
        }
    }
}
