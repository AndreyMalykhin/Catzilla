using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Catzilla.CommonModule.Config;

namespace Catzilla.AppModule.Config {
    public class AppContext: MVCSContext {
        private ModuleConfig[] moduleConfigs;

        public AppContext(MonoBehaviour view, ModuleConfig[] moduleConfigs)
            : base(view, false) {
            this.moduleConfigs = moduleConfigs;
            Start();
            Launch();
        }

        protected override void mapBindings() {
            for (int i = 0; i < moduleConfigs.Length; ++i) {
                moduleConfigs[i].InitBindings(injectionBinder);
            }

            injectionBinder.ReflectAll();

            for (int i = 0; i < moduleConfigs.Length; ++i) {
                moduleConfigs[i].PostBindings(injectionBinder);
            }
        }
    }
}
