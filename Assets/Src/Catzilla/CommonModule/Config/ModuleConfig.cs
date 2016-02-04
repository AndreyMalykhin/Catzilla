using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;

namespace Catzilla.CommonModule.Config {
    public abstract class ModuleConfig: ScriptableObject {
        public abstract void InitBindings(IInjectionBinder injectionBinder);
        public abstract void PostBindings(IInjectionBinder injectionBinder);
    }
}
