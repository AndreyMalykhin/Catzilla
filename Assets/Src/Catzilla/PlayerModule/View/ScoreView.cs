using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Catzilla.PlayerModule.View {
    public class ScoreView: strange.extensions.mediation.impl.View {
        [Inject("ScoreText")]
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
            RenderValue();
        }

        private void RenderValue() {
            text.text = string.Format(Text, value);
        }
    }
}
