using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, RestartBtnClick}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text RestartText;
        public Text ExitText;

        [SerializeField]
        private Button exitBtn;

        [SerializeField]
        private Button restartBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnReady() {
            // Debug.Log("GameOverMenuView.OnReady()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            exitBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            restartBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.RestartBtnClick);
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
