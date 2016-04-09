using UnityEngine;

namespace Catzilla.SkillModule.Model {
    public interface ISkillHelper {
        string GetSkillDescription(Skill skill, string description);
        void AddSkill(Skill skill, GameObject obj);
    }
}
