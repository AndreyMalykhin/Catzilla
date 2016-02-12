using UnityEngine;
using Zenject;

namespace Catzilla.CommonModule.View {
    public abstract class ModuleView: MonoBehaviour {
        public abstract void InitBindings(DiContainer container);
        public abstract void PostBindings(DiContainer container);
    }
}
