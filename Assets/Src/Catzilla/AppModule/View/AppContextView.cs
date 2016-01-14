using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using Catzilla.AppModule.Config;

namespace Catzilla.AppModule.View {
    public class AppContextView: ContextView {
        void Awake() {
            DontDestroyOnLoad(gameObject);
            context = new AppContext(this);
        }
    }
}
