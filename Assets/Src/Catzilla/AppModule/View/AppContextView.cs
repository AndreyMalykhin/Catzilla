using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.AppModule.Config;

namespace Catzilla.AppModule.View {
    public class AppContextView: ContextView {
        [SerializeField]
        private ModuleConfig[] moduleConfigs;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            context = new AppContext(this, moduleConfigs);
        }
    }
}
