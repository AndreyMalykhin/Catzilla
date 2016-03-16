using UnityEngine;
using System;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class WorldSpacePopupManager: ScriptableObject {
        [Inject]
        [NonSerialized]
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
            // DebugUtils.Log("WorldSpacePopupManager.Show()");
            for (int i = 0; i < recentPopups.Length; ++i) {
                if (recentPopups[i] == popup) {
                    recentPopups[i] = null;
                    break;
                }
            }

            WorldSpacePopupView recentPopup = recentPopups[nextPopupIndex];

            if (recentPopup != null) {
                recentPopup.GetComponent<ShowableView>().Hide();
            }

            recentPopups[nextPopupIndex] = popup;
            ++nextPopupIndex;

            if (nextPopupIndex >= recentPopups.Length) {
                nextPopupIndex = 0;
            }

            var showable = popup.GetComponent<ShowableView>();
            DebugUtils.Assert(!showable.IsShown);
            showable.OnHide = OnPopupHide;
            showable.Show();
        }

        private void OnPopupHide(ShowableView showable) {
            poolStorage.Return(showable.GetComponent<PoolableView>());
        }
    }
}
