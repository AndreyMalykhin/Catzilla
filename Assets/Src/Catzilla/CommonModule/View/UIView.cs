using UnityEngine;

namespace Catzilla.CommonModule.View {
    public class UIView: MonoBehaviour {
        public AudioSource AudioSource {get {return audioSource;}}

        [SerializeField]
        private AudioSource audioSource;
    }
}
