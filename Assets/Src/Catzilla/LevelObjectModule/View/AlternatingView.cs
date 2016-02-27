using UnityEngine;

namespace Catzilla.LevelObjectModule.View {
    public class AlternatingView: MonoBehaviour {
        public Material Material {
            get {return renderers[0].sharedMaterial;}
            set {
                for (int i = 0; i < renderers.Length; ++i) {
                    renderers[i].sharedMaterial = value;
                }
            }
        }

        [SerializeField]
        private Renderer[] renderers;
    }
}
