using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.AppModule.View {
    public class AppView: MonoInstaller {
        [SerializeField]
        private ModuleView[] modules;

        public override void InstallBindings() {
            for (int i = 0; i < modules.Length; ++i) {
                modules[i].InitBindings(Container);
            }
        }

        public override void Start() {
            for (int i = 0; i < modules.Length; ++i) {
                modules[i].PostBindings(Container);
            }

            var eventBus = Container.Resolve<EventBus>();
            eventBus.Fire((int) Events.AppStart, new Evt(this));
        }

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
