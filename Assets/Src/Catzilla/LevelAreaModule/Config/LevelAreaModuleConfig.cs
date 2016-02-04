using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelAreaModule.Controller;

namespace Catzilla.LevelAreaModule.Config {
    [CreateAssetMenuAttribute]
    public class LevelAreaModuleConfig: ModuleConfig {
        [SerializeField]
        private EnvTypeInfoStorage envTypeInfoStorage;

        public override void InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<LevelAreaGenerator>()
                .To<LevelAreaGenerator>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelAreaController>()
                .To<LevelAreaController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<EnvTypeInfoStorage>()
                .ToValue(envTypeInfoStorage)
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(1 << 10)
                .ToName("EnvLayer")
                .ToInject(false)
                .CrossContext();
        }

        public override void PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();
            var levelAreaController =
                injectionBinder.GetInstance<LevelAreaController>();
            eventBus.On(
                LevelView.Event.Construct, levelAreaController.OnLevelConstruct);
            eventBus.On(LevelAreaView.Event.TriggerEnter,
                levelAreaController.OnTriggerEnter);
        }
    }
}
