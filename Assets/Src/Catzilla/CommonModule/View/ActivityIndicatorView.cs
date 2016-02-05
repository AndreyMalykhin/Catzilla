using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ActivityIndicatorView: MonoBehaviour {
        private Canvas canvas;
        private Animator animator;

        public void Show() {
            // DebugUtils.Log("ActivityIndicatorView.Show()");
            canvas.enabled = true;
            animator.enabled = true;
        }

        public void Hide() {
            // DebugUtils.Log("ActivityIndicatorView.Hide()");
            canvas.enabled = false;
            animator.enabled = false;
        }

        private void Awake() {
            canvas = GetComponent<Canvas>();
            animator = GetComponent<Animator>();
            canvas.enabled = false;
            animator.enabled = false;
        }
    }
}
