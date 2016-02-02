using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelModule.View {
    public class LevelStartScreenView: strange.extensions.mediation.impl.View {
        public enum Event {Hide}

        [Inject]
        public EventBus EventBus {get; set;}

        public Text Msg;

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
        }

        private IEnumerator Hide() {
            yield return new WaitForSeconds(duration);
            canvas.enabled = false;
            EventBus.Fire(Event.Hide, new Evt(this));
        }
    }
}
