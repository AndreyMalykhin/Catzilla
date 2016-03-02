using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.View {
    public class HUDHealthView: MonoBehaviour {
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

        private static readonly int increaseParam =
            Animator.StringToHash("Increase");
        private static readonly int decreaseParam =
            Animator.StringToHash("Decrease");

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        [Tooltip("In seconds")]
        private float animationDuration = 2f;

        private IEnumerator setValueCoroutine;

        private IEnumerator SetValue(int value) {
            // DebugUtils.Log("HUDHealthView.SetValue()");
            float elapsedTime = 0f;
            float startValue = slider.value;

            if (animator != null) {
                animator.SetTrigger(
                    value >= slider.value ? increaseParam : decreaseParam);
            }

            while (elapsedTime < animationDuration) {
                float completionPercentage = elapsedTime / animationDuration;
                slider.value = Mathf.Lerp(
                    startValue, value, completionPercentage);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            slider.value = value;
        }

        private void Awake() {
            slider.value = 0f;
        }
    }
}
