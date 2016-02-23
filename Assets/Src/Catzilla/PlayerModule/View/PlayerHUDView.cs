using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.PlayerModule.View {
    public class PlayerHUDView: MonoBehaviour {
        [Inject]
        public UIView UI {get; set;}

        public PlayerHealthView Health;
        public PlayerScoreView Score;

        [PostInject]
        public void OnConstruct() {
            bool isWorldPositionStays = false;
            var transform = (RectTransform) this.transform;
            transform.SetParent(UI.transform, isWorldPositionStays);
            transform.anchorMax = new Vector2(1f, 1f);
            transform.offsetMax = new Vector2(0f, 0f);
        }
    }
}
