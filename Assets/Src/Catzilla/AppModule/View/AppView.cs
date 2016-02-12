using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.AppModule.View {
    public class AppView: MonoInstaller {
        public enum Event {Start}

        [SerializeField]
        private ModuleView[] modules;

        public override void InstallBindings() {
            for (int i = 0; i < modules.Length; ++i) {
                modules[i].InitBindings(Container);
            }

            for (int i = 0; i < modules.Length; ++i) {
                modules[i].PostBindings(Container);
            }
        }

        public override void Start() {
            var eventBus = Container.Resolve<EventBus>();
            eventBus.Fire(Event.Start, new Evt(this));
        }

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
