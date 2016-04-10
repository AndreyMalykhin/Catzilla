using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public static class SkillsScreenHelper {
        private static readonly List<SkillListItemView.Item> itemsBuffer =
            new List<SkillListItemView.Item>(8);
        private static readonly IDictionary<int, Skill> currentSkillsBuffer =
            new Dictionary<int, Skill>(8);

        public static List<SkillListItemView.Item> GetItems(
            SkillStorage skillStorage,
            PlayerStateStorage playerStateStorage,
            SkillHelperStorage skillHelperStorage,
            Translator translator) {
            itemsBuffer.Clear();
            currentSkillsBuffer.Clear();
            BaseSkill[] baseSkills = skillStorage.GetAllBase();
            PlayerState playerState = playerStateStorage.Get();
            List<int> currentSkillIds = playerState.SkillIds;

            for (int i = 0; i < currentSkillIds.Count; ++i) {
                Skill currentSkill = skillStorage.Get(currentSkillIds[i]);
                currentSkillsBuffer.Add(currentSkill.BaseId, currentSkill);
            }

            for (int i = 0; i < baseSkills.Length; ++i) {
                itemsBuffer.Add(GetItem(
                    baseSkills[i],
                    currentSkillsBuffer,
                    playerState,
                    skillStorage,
                    skillHelperStorage,
                    translator
                ));
            }

            return itemsBuffer;
        }

        private static SkillListItemView.Item GetItem(
            BaseSkill baseSkill,
            IDictionary<int, Skill> currentSkills,
            PlayerState playerState,
            SkillStorage skillStorage,
            SkillHelperStorage skillHelperStorage,
            Translator translator) {
            ISkillHelper skillHelper =
                skillHelperStorage.Get(baseSkill.Type);
            Skill currentSkill = null;
            var currentSkillLevel = -1;
            var currentSkillDescription = "";
            Skill nextSkill = null;

            if (currentSkillsBuffer.TryGetValue(
                    baseSkill.Id, out currentSkill)) {
                currentSkillLevel = currentSkill.Level;
                currentSkillDescription = translator.Translate(
                    "Skill.CurrentLevelDescription",
                    skillHelper.GetSkillDescription(
                        currentSkill,
                        translator.Translate(baseSkill.Description)
                    )
                );
                nextSkill =
                    skillStorage.GetNextLevel(currentSkill.Id);
            } else {
                nextSkill = skillStorage.GetFirstLevel(baseSkill.Id);
            }

            int nextSkillId = -1;
            string nextSkillDescription = "";

            if (nextSkill != null) {
                nextSkillId = nextSkill.Id;
                nextSkillDescription = translator.Translate(
                    "Skill.NextLevelDescription",
                    skillHelper.GetSkillDescription(
                        nextSkill,
                        translator.Translate(baseSkill.Description)
                    )
                );
            }

            int maxSkillLevel =
                skillStorage.GetMaxLevel(baseSkill.Id).Level + 1;
            string currentSkillLevelText = translator.Translate(
                "Skill.CurrentLevel",
                currentSkillLevel + 1,
                maxSkillLevel);
            bool isSkillDisabled =
                playerState.AvailableSkillPointsCount <= 0;
            return new SkillListItemView.Item{
                Img = baseSkill.Img,
                Name = translator.Translate(baseSkill.Name),
                CurrentLevel = currentSkillLevelText,
                CurrentLevelDescription = currentSkillDescription,
                NextLevelId = nextSkillId,
                NextLevelDescription = nextSkillDescription,
                IsDisabled = isSkillDisabled,
                Learn = translator.Translate("Skill.Learn")
            };
        }
    }
}
