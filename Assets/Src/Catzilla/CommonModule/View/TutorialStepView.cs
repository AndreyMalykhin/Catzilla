using UnityEngine;
using UnityEngine.UI;

namespace Catzilla.CommonModule.View {
    public class TutorialStepView: MonoBehaviour {
        public Button NextBtn {get {return nextBtn;}}

        [SerializeField]
        private Button nextBtn;
    }
}
