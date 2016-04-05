using UnityEngine;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class HUDNotificationsView: MonoBehaviour {
        public TextMesh Msg {get {return msg;}}

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private TextMesh msg;

        [PostInject]
        public void OnConstruct() {
            eventBus.Fire(
                (int) Events.HUDNotificationsConstruct, new Evt(this));
        }
    }
}
