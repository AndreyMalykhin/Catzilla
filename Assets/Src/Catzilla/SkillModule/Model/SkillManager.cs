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

        [Inject]
        private SkillsScreenView skillsScreen;

        public void ShowScreen() {
            skillsScreen.Items = SkillsScreenHelper.GetItems(
                skillStorage,
                playerStateStorage,
                skillHelperStorage,
                translator);
            skillsScreen.GetComponent<ShowableView>().Show();
        }
    }
}
