using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.SkillModule.Model;
using Catzilla.SkillModule.View;

namespace Catzilla.SkillModule.Controller {
    public class SkillsScreenController {
        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private SkillManager skillManager;

        [Inject]
        private SkillStorage skillStorage;

        [Inject]
        private SkillHelperStorage skillHelperStorage;

        [Inject]
        private Translator translator;

        [Inject]
        private SkillsScreenView skillsScreen;

        [PostInject]
        public void OnConstruct() {
            if (playerStateStorage.Get() != null) {
                UpdateScreen();
            }
        }

        public void OnLearnBtnClick(Evt evt) {
            var skillId = (int) evt.Data;
            skillManager.Learn(skillId);
        }

        public void OnCloseBtnClick() {
            skillsScreen.GetComponent<ShowableView>().Hide();
        }

        public void OnPlayerSave(Evt evt) {
            UpdateScreen();
        }

        private void UpdateScreen() {
            skillsScreen.AvailableSkillPoints.text = translator.Translate(
                "SkillsScreen.AvailableSkillPoints",
                playerStateStorage.Get().AvailableSkillPointsCount);
            skillsScreen.Items = SkillsScreenHelper.GetItems(
                skillStorage,
                playerStateStorage,
                skillHelperStorage,
                translator);
        }
    }
}
