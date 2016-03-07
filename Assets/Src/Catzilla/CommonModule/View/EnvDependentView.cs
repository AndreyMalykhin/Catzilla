using UnityEngine;

namespace Catzilla.CommonModule.View {
    public class EnvDependentView: MonoBehaviour {
        [SerializeField]
        private bool isDisabledInDev;

        [SerializeField]
        private bool isDisabledInProd;

        private void Awake() {
            if (isDisabledInDev && Debug.isDebugBuild) {
                gameObject.SetActive(false);
            }

            if (isDisabledInProd && !Debug.isDebugBuild) {
                gameObject.SetActive(false);
            }
        }
    }
}
