using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.MainMenuModule.View {
    public class MainMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, StartBtnClick}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text StartText;
        public Text ExitText;

        [SerializeField]
        private Button startBtn;

        [SerializeField]
        private Button exitBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            // Debug.Log("MainMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            exitBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            startBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.StartBtnClick);
            });
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }
    }
}
