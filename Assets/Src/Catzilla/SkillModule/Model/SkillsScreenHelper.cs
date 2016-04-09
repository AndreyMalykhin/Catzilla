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
            BaseSkill[] baseSkills = skillStorage.GetAllBase();
            PlayerState playerState = playerStateStorage.Get();
            List<int> currentSkillIds = playerState.SkillIds;

            for (int i = 0; i < currentSkillIds.Count; ++i) {
                Skill currentSkill = skillStorage.Get(currentSkillIds[i]);
                currentSkillsBuffer.Add(currentSkill.BaseId, currentSkill);
            }

            for (int i = 0; i < baseSkills.Length; ++i) {
                BaseSkill baseSkill = baseSkills[i];
                ISkillHelper skillHelper =
                    skillHelperStorage.Get(baseSkill.Type);
                Skill currentSkill = null;
                var currentSkillLevel = -1;
                var currentSkillDescription = "";
                Skill nextSkill = null;

                if (currentSkillsBuffer.TryGetValue(
                        baseSkill.Id, out currentSkill)) {
                    currentSkillLevel = currentSkill.Level;
                    currentSkillDescription = skillHelper.GetSkillDescription(
                        currentSkill, baseSkill.Description);
                    nextSkill =
                        skillStorage.GetNextLevel(currentSkill.Id);
                } else {
                    nextSkill = skillStorage.GetFirstLevel(baseSkill.Id);
                }

                string nextSkillDescription = skillHelper.GetSkillDescription(
                    nextSkill, baseSkill.Description);
                int maxSkillLevel =
                    skillStorage.GetMaxLevel(baseSkill.Id).Level;
                bool isSkillAvailable =
                    playerState.AvailableSkillPointsCount > 0;
                itemsBuffer.Add(new SkillListItemView.Item(
                    baseSkill.Id,
                    baseSkill.Img,
                    translator.Translate(baseSkill.Name),
                    currentSkillLevel,
                    currentSkillDescription,
                    maxSkillLevel,
                    nextSkill.Id,
                    nextSkillDescription,
                    isSkillAvailable
                ));
            }

            itemsBuffer.Clear();
            currentSkillsBuffer.Clear();
            return itemsBuffer;
        }
    }
}
