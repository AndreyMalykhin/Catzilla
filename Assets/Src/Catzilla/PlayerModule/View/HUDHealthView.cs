using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class HUDHealthView: MonoBehaviour {
        public int Value {
            set {
                if (this.value == value) {
                    return;
                }

                if (animator != null) {
                    animator.SetTrigger(
                        value > this.value ? increaseParam : decreaseParam);
                }

                this.value = value;
            }
        }

        public int MaxValue {set {slider.maxValue = value;}}

        private static readonly int increaseParam =
            Animator.StringToHash("Increase");
        private static readonly int decreaseParam =
            Animator.StringToHash("Decrease");

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float changeSpeed;

        private float value;

        private void Update() {
            if (value == slider.value) {
                return;
            }

            slider.value = Mathf.MoveTowards(
                slider.value, value, changeSpeed * Time.deltaTime);
        }

        private void Awake() {
            slider.value = slider.maxValue;
        }
    }
}
