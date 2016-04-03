using UnityEngine;
using UnityEngine.UI;
using System;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class CanvasGroupBasedDeactivatableView: DeactivatableView {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private CanvasScaler canvasScaler;

        public override bool IsActive {
            get {return canvasGroup.alpha != 0f;}
            set {
                if (canvasScaler != null) {
                    canvasScaler.enabled = value;
                }

                canvasGroup.alpha = value ? 1f : 0f;
            }
        }
    }
}
