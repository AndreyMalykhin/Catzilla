using UnityEngine;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class HUDNotificationsView: MonoBehaviour {
        public ScreenSpacePopupManagerView PopupManager {
            get {return popupManager;}
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private ScreenSpacePopupManagerView popupManager;

        [PostInject]
        public void OnConstruct() {
            eventBus.Fire(
                (int) Events.HUDNotificationsConstruct, new Evt(this));
        }
    }
}
