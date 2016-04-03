using UnityEngine;
using UnityEngine.UI;
using System;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class CanvasBasedDeactivatableView: DeactivatableView {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private CanvasScaler canvasScaler;

        public override bool IsActive {
            get {return canvas.enabled;}
            set {
                if (canvasScaler != null) {
                    canvasScaler.enabled = value;
                }

                canvas.enabled = value;
            }
        }
    }
}
