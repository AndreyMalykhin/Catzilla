using UnityEngine;
using Zenject;

namespace Catzilla.CommonModule.Config {
    public abstract class ModuleConfig: ScriptableObject {
        public abstract void InitBindings(DiContainer container);
        public abstract void PostBindings(DiContainer container);
    }
}
