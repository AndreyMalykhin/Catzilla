using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class PlayerScoreView: MonoBehaviour {
        public enum Event {Construct}

        [Inject]
        public EventBus EventBus {get; set;}

        public string Text {
            get {return text;}
            set {text = value;}
        }

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
        private string text = "Score: {0} / {1}";
        private Text textView;

        [PostInject]
        public void OnConstruct() {
            textView = GetComponent<Text>();
            Render();
            EventBus.Fire(Event.Construct, new Evt(this));
        }

        private void Render() {
            textView.text = string.Format(text, value, maxValue);
        }
    }
}
