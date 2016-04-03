using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaView: MonoBehaviour, IPoolable {
        [Inject]
        private EventBus eventBus;

        public int Index {get; private set;}

        public void Init(int index) {
            Index = index;
        }

        void IPoolable.OnReturn() {
            eventBus.Fire((int) Events.LevelAreaDestroy, new Evt(this));
        }

        void IPoolable.OnTake() {}
    }
}
