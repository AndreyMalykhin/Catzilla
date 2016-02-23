using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class PlayerHealthView: MonoBehaviour {
        public int Value {
            set {
                if (slider.value == value) {
                    return;
                }

                if (setValueCoroutine != null) {
                    StopCoroutine(setValueCoroutine);
                }

                setValueCoroutine = SetValue(value);
                StartCoroutine(setValueCoroutine);
            }
        }

        public int MaxValue {set {slider.maxValue = value;}}

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private float animationDuration = 2f;

        [SerializeField]
        private Color increaseColor = Color.white;

        [SerializeField]
        private Color decreaseColor = Color.red;

        [SerializeField]
        private Image fillImg;

        private IEnumerator setValueCoroutine;
        private Color normalColor;

        [PostInject]
        public void OnConstruct() {
            normalColor = fillImg.color;
        }

        private IEnumerator SetValue(int value) {
            // DebugUtils.Log("PlayerHealthView.SetValue()");
            float elapsedTime = 0f;
            float startValue = slider.value;
            Color startColor =
                value >= slider.value ? increaseColor : decreaseColor;

            while (elapsedTime < animationDuration) {
                float completionPercentage = elapsedTime / animationDuration;
                slider.value = Mathf.Lerp(
                    startValue, value, completionPercentage);
                fillImg.color = Color.Lerp(
                    startColor, normalColor, completionPercentage);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            slider.value = value;
            fillImg.color = normalColor;
        }
    }
}
