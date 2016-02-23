using UnityEngine;
using UnityEngine.UI;

namespace Catzilla.PlayerModule.View {
    public class PlayerHealthView: MonoBehaviour {
        public int Value {
            get {return (int) slider.value;}
            set {slider.value = value;}
        }

        public int MaxValue {
            get {return (int) slider.maxValue;}
            set {slider.maxValue = value;}
        }

        [SerializeField]
        private Slider slider;
    }
}
