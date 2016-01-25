using UnityEngine;
using System.Collections;

namespace Catzilla.CommonModule.View {
    public class AudioSourceView: MonoBehaviour {
        [SerializeField]
        private bool ignoreListenerPause;

        private void Awake() {
            GetComponent<AudioSource>().ignoreListenerPause =
                ignoreListenerPause;
        }
    }
}
