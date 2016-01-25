using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class BtnView: strange.extensions.mediation.impl.View {
        public enum Event {Click}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public AudioClip ClickSound;
        public AudioSource AudioSource;

        [PostConstruct]
        public void OnConstruct() {
            GetComponent<Button>().onClick.AddListener(() => {
                EventBus.Dispatch(Event.Click, this);
            });
        }
    }
}
