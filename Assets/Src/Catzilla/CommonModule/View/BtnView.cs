using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class BtnView: strange.extensions.mediation.impl.View {
        public enum Event {Click}

        [Inject]
        public EventBus EventBus {get; set;}

        public AudioClip ClickSound;
        public AudioSource AudioSource;

        [PostConstruct]
        public void OnConstruct() {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick() {
            EventBus.Fire(Event.Click, new Evt(this));
        }
    }
}
