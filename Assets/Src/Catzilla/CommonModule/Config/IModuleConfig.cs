using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;

namespace Catzilla.CommonModule.Config {
    public interface IModuleConfig {
        void InitBindings(IInjectionBinder injectionBinder);
        void PostBindings(IInjectionBinder injectionBinder);
    }
}
