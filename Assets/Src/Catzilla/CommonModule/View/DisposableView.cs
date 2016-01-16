using UnityEngine;
using System.Collections;

namespace Catzilla.CommonModule.View {
    public class DisposableView: strange.extensions.mediation.impl.View {
        [SerializeField]
        private GameObject root;

        public void Init(GameObject root) {
            this.root = root;
        }

        private void OnBecameInvisible() {
            Dispose();
        }

        private void Dispose() {
            // Debug.Log("DisposableView.Dispose()");
            Destroy(root == null ? gameObject : root);
        }
    }
}
