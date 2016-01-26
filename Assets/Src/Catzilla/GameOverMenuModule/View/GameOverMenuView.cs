using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, RestartBtnClick, ResurrectBtnClick}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text RestartText;
        public Text ExitText;
        public Text ResurrectText;
        public Button ExitBtn;
        public Button RestartBtn;
        public Button ResurrectBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            // Debug.Log("GameOverMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            ExitBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            RestartBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.RestartBtnClick);
            });
            ResurrectBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ResurrectBtnClick);
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
