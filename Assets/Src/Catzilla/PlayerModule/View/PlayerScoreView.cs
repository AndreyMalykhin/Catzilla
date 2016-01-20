using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.PlayerModule.View {
    public class PlayerScoreView: strange.extensions.mediation.impl.View {
        public enum Event {Construct}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public string Text {get; set;}

        public int Value {
            get {
                return value;
            }
            set {
                this.value = value;
                Render();
            }
        }

        public int MaxValue {
            get {
                return maxValue;
            }
            set {
                this.maxValue = value;
                Render();
            }
        }

        private int value;
        private int maxValue;
        private Text text;

        [PostConstruct]
        public void OnConstruct() {
            text = GetComponent<Text>();
            Text = "Score: {0} / {1}";
            Render();
            EventBus.Dispatch(Event.Construct, this);
        }

        private void Render() {
            text.text = string.Format(Text, value, maxValue);
        }
    }
}
