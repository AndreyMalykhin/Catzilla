using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class HUDScoreView: MonoBehaviour {
        public int Value {
            get {return value;}
            set {
                this.value = value;
                Render();
            }
        }

        public int MaxValue {
            get {return maxValue;}
            set {
                this.maxValue = value;
                Render();
            }
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private Text text;

        private int value;
        private int maxValue;
        private readonly StringBuilder strBuilder = new StringBuilder(16);

        [PostInject]
        public void OnConstruct() {
            Render();
            eventBus.Fire((int) Events.HUDScoreConstruct, new Evt(this));
        }

        private void Render() {
            Profiler.BeginSample("HUDScoreView.Render()");
            text.text = strBuilder.Append(value)
                .Append("/")
                .Append(maxValue)
                .ToString();
            strBuilder.Length = 0;
            Profiler.EndSample();
        }
    }
}
