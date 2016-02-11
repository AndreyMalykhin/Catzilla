using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.AppModule.Config;

namespace Catzilla.AppModule.View {
    public class AppView: MonoInstaller {
        public enum Event {Start}

        [SerializeField]
        private ModuleConfig[] moduleConfigs;

        public override void InstallBindings() {
            for (int i = 0; i < moduleConfigs.Length; ++i) {
                moduleConfigs[i].InitBindings(Container);
            }

            for (int i = 0; i < moduleConfigs.Length; ++i) {
                moduleConfigs[i].PostBindings(Container);
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
