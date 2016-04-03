using UnityEngine;
using System;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class LayerBasedDeactivatableView: DeactivatableView {
        [Serializable]
        private struct Child {
            public GameObject Object;
            public int ActiveLayer;
        }

        [SerializeField]
        private int inactiveLayer;

        private Child[] children;

        public override bool IsActive {
            get {return children[0].Object.layer != inactiveLayer;}
            set {
                if (value) {
                    for (int i = 0; i < children.Length; ++i) {
                        Child child = children[i];
                        child.Object.layer = child.ActiveLayer;
                    }

                    return;
                }

                for (int i = 0; i < children.Length; ++i) {
                    children[i].Object.layer = inactiveLayer;
                }
            }
        }

        private void Awake() {
            var childrenComponents = GetComponentsInChildren<Transform>();
            int childrenCount = childrenComponents.Length;
            children = new Child[childrenCount];

            for (int i = 0; i < childrenCount; ++i) {
                GameObject obj = childrenComponents[i].gameObject;
                children[i] = new Child{Object = obj, ActiveLayer = obj.layer};
            }
        }
    }
}
