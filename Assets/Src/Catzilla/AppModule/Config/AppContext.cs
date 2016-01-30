using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Catzilla.CommonModule.Config;
using Catzilla.MainMenuModule.Config;
using Catzilla.LeaderboardModule.Config;
using Catzilla.GameOverMenuModule.Config;
using Catzilla.LevelModule.Config;
using Catzilla.LevelAreaModule.Config;
using Catzilla.LevelObjectModule.Config;
using Catzilla.PlayerModule.Config;

namespace Catzilla.AppModule.Config {
    public class AppContext: MVCSContext {
        public AppContext(MonoBehaviour view): base(view) {}

		public AppContext(MonoBehaviour view, ContextStartupFlags flags)
            : base(view, flags) {}

        protected override void mapBindings() {
            IModuleConfig[] modules = {
                new CommonModuleConfig(),
                new MainMenuModuleConfig(),
                new GameOverMenuModuleConfig(),
                new LeaderboardModuleConfig(),
                new PlayerModuleConfig(),
                new LevelObjectModuleConfig(),
                new LevelAreaModuleConfig(),
                new LevelModuleConfig(),
                new AppModuleConfig()
            };

            for (int i = 0; i < modules.Length; ++i) {
                modules[i].InitBindings(injectionBinder);
            }

            injectionBinder.ReflectAll();

            for (int i = 0; i < modules.Length; ++i) {
                modules[i].PostBindings(injectionBinder);
            }
        }
    }
}
