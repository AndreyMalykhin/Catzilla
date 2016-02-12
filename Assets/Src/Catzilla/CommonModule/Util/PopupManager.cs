using UnityEngine;
using System;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class PopupManager: ScriptableObject {
        [SerializeField]
        private int simultaneousPopupsCount = 4;

        [NonSerialized]
        private WorldSpacePopupView[] recentPopups;

        [NonSerialized]
        private int nextPopupIndex;

        public void Show(WorldSpacePopupView popup) {
            WorldSpacePopupView recentPopup = recentPopups[nextPopupIndex];

            if (recentPopup != null) {
                recentPopup.Hide();
            }

            recentPopups[nextPopupIndex] = popup;
            ++nextPopupIndex;

            if (nextPopupIndex >= recentPopups.Length) {
                nextPopupIndex = 0;
            }

            popup.Show();
        }

        private void OnEnable() {
            recentPopups = new WorldSpacePopupView[simultaneousPopupsCount];
        }
    }
}
