using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.SkillModule.Model {
    [CreateAssetMenuAttribute]
    public class SkillStorage: ScriptableObject {
        [Serializable]
        private struct Item {
            public BaseSkill BaseSkill;
            public Skill[] Skills;
        }

        [SerializeField]
        private Item[] items;

        [NonSerialized]
        private readonly IDictionary<int, Skill> skillsMap =
            new Dictionary<int, Skill>(32);

        [NonSerialized]
        private readonly IDictionary<int, Item> baseSkillsMap =
            new Dictionary<int, Item>(8);

        [NonSerialized]
        private BaseSkill[] baseSkillsList;

        public Skill Get(int id) {
            return skillsMap[id];
        }

        public BaseSkill GetBase(int baseId) {
            return baseSkillsMap[baseId].BaseSkill;
        }

        public BaseSkill[] GetAllBase() {
            return baseSkillsList;
        }

        public Skill GetFirstLevel(int baseId) {
            return baseSkillsMap[baseId].Skills[0];
        }

        /**
         * @return Null if no next level
         */
        public Skill GetNextLevel(int id) {
            Skill skill = skillsMap[id];
            Skill[] skills = baseSkillsMap[skill.BaseId].Skills;

            for (int i = 0; i < skills.Length; ++i) {
                if (skills[i].Id == id) {
                    return i == skills.Length - 1 ? null : skills[i + 1];
                }
            }

            DebugUtils.Assert(false);
            return null;
        }

        public Skill GetMaxLevel(int baseId) {
            Skill[] skills = baseSkillsMap[baseId].Skills;
            return skills[skills.Length - 1];
        }

        private void OnEnable() {
            baseSkillsList = new BaseSkill[items.Length];

            for (int i = 0; i < items.Length; ++i) {
                Item item = items[i];
                BaseSkill baseSkill = item.BaseSkill;
                baseSkillsMap.Add(baseSkill.Id, item);
                baseSkillsList[i] = baseSkill;
                Skill[] derivedSkills = item.Skills;
                Array.Sort(derivedSkills, SkillComparer);

                for (int j = 0; j < derivedSkills.Length; ++j) {
                    Skill skill = derivedSkills[j];
                    DebugUtils.Assert(skill.BaseId == baseSkill.Id);
                    skillsMap.Add(skill.Id, skill);
                }
            }

            Array.Sort(baseSkillsList, BaseSkillComparer);
        }

        private int SkillComparer(Skill lhs, Skill rhs) {
            return lhs.Level.CompareTo(rhs.Level);
        }

        private int BaseSkillComparer(BaseSkill lhs, BaseSkill rhs) {
            return lhs.Order.CompareTo(rhs.Order);
        }
    }
}
