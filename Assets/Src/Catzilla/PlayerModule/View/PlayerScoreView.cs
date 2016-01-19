using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.PlayerModule.View {
    public class PlayerScoreView: strange.extensions.mediation.impl.View {
        public enum Event {Ready}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public string Text {get; set;}

        public int Value {
            get {
                return value;
            }
            set {
                this.value = value;
                RenderValue();
            }
        }

        private int value;
        private Text text;

        [PostConstruct]
        public void OnReady() {
            text = GetComponent<Text>();
            Text = "Score: {0}";
            RenderValue();
            EventBus.Dispatch(Event.Ready, this);
        }

        private void RenderValue() {
            text.text = string.Format(Text, value);
        }
    }
}
