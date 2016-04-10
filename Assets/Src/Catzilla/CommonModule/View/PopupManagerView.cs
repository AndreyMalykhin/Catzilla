using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    abstract public class PopupManagerView: MonoBehaviour {
        [Serializable]
        private struct PopupType {
            public int Id;
            public PopupView Proto;
        }

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private int simultaneousPopupsCount;

        [SerializeField]
        private PopupType[] popupTypes;

        [NonSerialized]
        private int nextPopupIndex;

        [NonSerialized]
        private PopupView[] recentPopups;

        private readonly IDictionary<int, PopupView> popupProtos =
            new Dictionary<int, PopupView>(8);

        [PostInject]
        public void OnConstruct() {
            recentPopups = new PopupView[simultaneousPopupsCount];

            for (int i = 0; i < popupTypes.Length; ++i) {
                PopupType popupType = popupTypes[i];
                popupProtos.Add(popupType.Id, popupType.Proto);
            }
        }

        public PopupView Get(int popupType) {
            PopupView popupProto = popupProtos[popupType];
            int poolId = popupProto.GetComponent<PoolableView>().PoolId;
            return poolStorage.Take(poolId)
                .GetComponent<PopupView>();
        }

        public void Show(PopupView popup) {
            // DebugUtils.Log("PopupManagerView.Show()");
            OnPreShow(popup);

            for (int i = 0; i < recentPopups.Length; ++i) {
                if (recentPopups[i] == popup) {
                    recentPopups[i] = null;
                    break;
                }
            }

            PopupView recentPopup = recentPopups[nextPopupIndex];

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

        protected virtual void OnPreShow(PopupView popup) {}

        private void OnPopupHide(ShowableView showable) {
            poolStorage.Return(showable.GetComponent<PoolableView>());
        }
    }
}
