using UnityEngine;
using System.Collections;
using Catzilla.CommonModule.View;

namespace Catzilla.PlayerModule.View {
    public class PlayerHUDView: strange.extensions.mediation.impl.View {
        [Inject]
        public UIView UI {get; set;}

        [PostConstruct]
        public void OnReady() {
            bool isWorldPositionStays = false;
            var transform = (RectTransform) this.transform;
            transform.SetParent(UI.transform, isWorldPositionStays);
            transform.anchorMax = new Vector2(1f, 1f);
            transform.offsetMax = new Vector2(0f, 0f);
        }
    }
}
