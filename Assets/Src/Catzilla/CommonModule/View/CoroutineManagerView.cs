using UnityEngine;
using System.Collections;

namespace Catzilla.CommonModule.View {
    public class CoroutineManagerView: MonoBehaviour {
        public Coroutine Run(IEnumerator coroutine) {
            return StartCoroutine(coroutine);
        }
    }
}
