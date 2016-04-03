using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class WorldSpaceTextPopupView: PopupView, IPoolable {
        public Camera LookAtTarget {
            get {return lookAtTarget;}
            set {lookAtTarget = value;}
        }

        public TextMesh Msg {get {return msg;}}

        public Vector3 Offset;

        [SerializeField]
        new private Renderer renderer;

        [SerializeField]
        private TextMesh msg;

        private Camera lookAtTarget;

        public void PlaceAbove(Bounds bounds) {
            Vector3 boundsCenter = bounds.center;
            transform.position = new Vector3(
                boundsCenter.x,
                bounds.max.y,
                boundsCenter.z) + Offset;
            FitIntoCamera();
        }

        void IPoolable.OnReturn() {
            lookAtTarget = null;
        }

		void IPoolable.OnTake() {}

        private void FitIntoCamera() {
            if (lookAtTarget == null) {
                return;
            }

            Bounds bounds = renderer.bounds;
            float popupRight = bounds.max.x;
            float popupLeft = bounds.min.x;
            float distance =
                lookAtTarget.transform.position.y - transform.position.y;
            Vector3 screenRight = lookAtTarget.ViewportToWorldPoint(
                new Vector3(1f, 1f, distance));
            Vector3 screenLeft = lookAtTarget.ViewportToWorldPoint(
                new Vector3(0f, 0f, distance));

            if (popupLeft < screenLeft.x) {
                transform.Translate(screenLeft.x - popupLeft, 0f, 0f);
            } else if (popupRight > screenRight.x) {
                transform.Translate(screenRight.x - popupRight, 0f, 0f);
            }
        }

        private void LateUpdate() {
            if (lookAtTarget == null) {
                return;
            }

            transform.rotation =
                Quaternion.LookRotation(lookAtTarget.transform.forward);
        }
    }
}
