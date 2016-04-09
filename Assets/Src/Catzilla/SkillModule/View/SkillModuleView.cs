using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelObjectModule.Controller;
using Catzilla.SkillModule.Model;
using Catzilla.SkillModule.Controller;

namespace Catzilla.SkillModule.View {
    public class SkillModuleView: ModuleView {
        [SerializeField]
        private SkillStorage skillStorage;

        [SerializeField]
        private SkillsScreenView skillsScreen;

        public override void InitBindings(DiContainer container) {
            container.Bind<SkillStorage>().ToInstance(skillStorage);
            container.Bind<SkillsScreenView>().ToInstance(skillsScreen);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<SkillStorage>());
            var eventBus = container.Resolve<EventBus>();
            var criticalScoreSkillController =
                container.Resolve<CriticalScoreSkillController>();
            eventBus.On((int) Events.CriticalScoreSkillConstruct,
                criticalScoreSkillController.OnConstruct);
        }
    }
}
