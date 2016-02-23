using UnityEngine;
using System;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class WorldSpacePopupManager: ScriptableObject {
        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private WorldSpacePopupView popupProto;

        [SerializeField]
        private int simultaneousPopupsCount = 4;

        [NonSerialized]
        private WorldSpacePopupView[] recentPopups;

        [NonSerialized]
        private int nextPopupIndex;

        [PostInject]
        public void OnConstruct() {
            recentPopups = new WorldSpacePopupView[simultaneousPopupsCount];
        }

        public WorldSpacePopupView Get() {
            int poolId = popupProto.GetComponent<PoolableView>().PoolId;
            return poolStorage.Take(poolId).GetComponent<WorldSpacePopupView>();
        }

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

            popup.OnHide = OnPopupHide;
            popup.Show();
        }

        private void OnPopupHide(WorldSpacePopupView popup) {
            poolStorage.Return(popup.GetComponent<PoolableView>());
        }
    }
}
