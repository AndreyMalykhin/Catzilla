using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaView: MonoBehaviour {
        [Inject]
        public EventBus EventBus {get; set;}

        public int Index {get; private set;}

        public void Init(int index) {
            Index = index;
        }

        private void OnTriggerEnter(Collider collider) {
            EventBus.Fire((int) Events.LevelAreaTriggerEnter,
                new Evt(this, collider));
        }
    }
}
