using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.LevelModule.View {
    public class LevelStartScreenView: strange.extensions.mediation.impl.View {
        public enum Event {Hide}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text Msg;

        [SerializeField]
        private float duration = 2f;

        private Canvas canvas;

        [PostConstruct]
        public void OnReady() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
            StartCoroutine(Hide());
        }

        private IEnumerator Hide() {
            yield return new WaitForSeconds(duration);
            canvas.enabled = false;
            EventBus.Dispatch(Event.Hide);
        }
    }
}
