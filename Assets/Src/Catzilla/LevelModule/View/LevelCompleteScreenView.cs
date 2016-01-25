using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.LevelModule.View {
    public class LevelCompleteScreenView
        : strange.extensions.mediation.impl.View {
        public enum Event {Hide, Show}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text Msg;
        public AudioClip ShowSound;
        public AudioSource AudioSource;

        [SerializeField]
        private float duration = 2f;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
            StartCoroutine(Hide());
            EventBus.Dispatch(Event.Show, this);
        }

        private IEnumerator Hide() {
            yield return new WaitForSeconds(duration);
            canvas.enabled = false;
            EventBus.Dispatch(Event.Hide, this);
        }
    }
}
