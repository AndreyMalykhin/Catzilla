using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.AppModule.Controller;
using Catzilla.AppModule.View;

namespace Catzilla.AppModule.Config {
    [CreateAssetMenuAttribute]
    public class AppModuleConfig: ModuleConfig {
        public override void InitBindings(DiContainer container) {
            container.Bind<AppController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var appController = container.Resolve<AppController>();
            eventBus.On(AppView.Event.Start, appController.OnStart);
        }
    }
}
