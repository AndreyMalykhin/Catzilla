using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.PlayerModule.View {
    public class HUDView: MonoBehaviour {
        public HUDHealthView Health;
        public HUDScoreView Score;

        [Inject("HUDWrapper")]
        private GameObject parent;

        [PostInject]
        public void OnConstruct() {
            bool isWorldPositionStays = false;
            var transform = (RectTransform) this.transform;
            transform.SetParent(parent.transform, isWorldPositionStays);
            transform.anchorMax = new Vector2(1f, 1f);
            transform.offsetMax = new Vector2(0f, 0f);
        }
    }
}
