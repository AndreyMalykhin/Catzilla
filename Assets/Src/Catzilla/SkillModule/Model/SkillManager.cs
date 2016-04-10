using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Model {
    public class SkillManager {
        [Inject]
        private SkillStorage skillStorage;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private SkillHelperStorage skillHelperStorage;

        [Inject]
        private Translator translator;

        public void Learn(int skillId) {
            PlayerState playerState = playerStateStorage.Get();
            List<int> currentSkillIds = playerState.SkillIds;

            for (int i = 0; i < currentSkillIds.Count; ++i) {
                var currentSkillId = currentSkillIds[i];

                if (skillStorage.Get(skillId).BaseId !=
                        skillStorage.Get(currentSkillId).BaseId) {
                    continue;
                }

                playerState.RemoveSkill(currentSkillId);
                break;
            }

            playerState.AddSkill(skillId);
            --playerState.AvailableSkillPointsCount;
            playerStateStorage.Save(playerState);
        }
    }
}
