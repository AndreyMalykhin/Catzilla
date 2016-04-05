using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaModuleView: ModuleView {
        [SerializeField]
        private EnvTypeInfoStorage envTypeInfoStorage;

        [SerializeField]
        private LevelAreaGeneratorView areaGenerator;

        public override void InitBindings(DiContainer container) {
            container.Bind<EnvTypeInfoStorage>().ToInstance(envTypeInfoStorage);
            container.Bind<LevelAreaGeneratorView>().ToInstance(areaGenerator);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<EnvTypeInfoStorage>());
        }
    }
}
