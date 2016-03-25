using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class WorldSpacePopupManager: ScriptableObject {
        [Serializable]
        private struct PopupType {
            public int Id;
            public WorldSpacePopupView Proto;
        }

        [Inject]
        [NonSerialized]
        private PoolStorageView poolStorage;

        [NonSerialized]
        private int nextPopupIndex;

        [NonSerialized]
        private WorldSpacePopupView[] recentPopups;

        [SerializeField]
        private int simultaneousPopupsCount = 4;

        [SerializeField]
        private PopupType[] popupTypes;

        private readonly IDictionary<int, WorldSpacePopupView> popupProtos =
            new Dictionary<int, WorldSpacePopupView>(4);

        [PostInject]
        public void OnConstruct() {
            recentPopups = new WorldSpacePopupView[simultaneousPopupsCount];

            for (int i = 0; i < popupTypes.Length; ++i) {
                PopupType popupType = popupTypes[i];
                popupProtos.Add(popupType.Id, popupType.Proto);
            }
        }

        public WorldSpacePopupView Get(int popupType) {
            WorldSpacePopupView popupProto = popupProtos[popupType];
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
            showable.OnHide += OnPopupHide;
            showable.Show();
        }

        private void OnPopupHide(ShowableView showable) {
            poolStorage.Return(showable.GetComponent<PoolableView>());
        }
    }
}
