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
            // DebugUtils.Log("SkillModuleView.InitBindings()");
            container.Bind<SkillsScreenController>().ToSingle();
            container.Bind<SkillManager>().ToSingle();
            container.Bind<CriticalSmashSkillController>().ToSingle();
            container.Bind<DamageAbsorptionSkillController>().ToSingle();
            container.Bind<SkillsScreenView>().ToInstance(skillsScreen);
            container.Bind<SkillStorage>()
                .ToSingleMethod((InjectContext context) => {
                    context.Container.Inject(skillStorage);
                    return skillStorage;
                });
            container.Bind<SkillHelperStorage>()
                .ToSingleMethod((InjectContext context) => {
                    var storage =
                        context.Container.Instantiate<SkillHelperStorage>();
                    storage.Add(
                        SkillType.CriticalSmash,
                        context.Container.Instantiate<CriticalSmashSkillHelper>());
                    storage.Add(
                        SkillType.DamageAbsorption,
                        context.Container.Instantiate<DamageAbsorptionSkillHelper>());
                    return storage;
                });
        }

        public override void PostBindings(DiContainer container) {
            // DebugUtils.Log("SkillModuleView.PostBindings()");
            var eventBus = container.Resolve<EventBus>();

            var criticalSmashSkillController =
                container.Resolve<CriticalSmashSkillController>();
            eventBus.On((int) Events.CriticalSmashSkillConstruct,
                criticalSmashSkillController.OnConstruct);

            var damageAbsorptionSkillController =
                container.Resolve<DamageAbsorptionSkillController>();
            eventBus.On((int) Events.DamageAbsorptionSkillConstruct,
                damageAbsorptionSkillController.OnConstruct);

            var skillsScreenController =
                container.Resolve<SkillsScreenController>();
            eventBus.On((int) Events.SkillListItemLearnBtnClick,
                skillsScreenController.OnLearnBtnClick);
            eventBus.On((int) Events.PlayerStateStorageSave,
                skillsScreenController.OnPlayerSave);
            var skillsScreen = container.Resolve<SkillsScreenView>();
            skillsScreen.CloseBtn.onClick.AddListener(
                skillsScreenController.OnCloseBtnClick);
        }
    }
}
