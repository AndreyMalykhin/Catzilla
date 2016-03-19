using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ScreenSpacePopupManagerView: MonoBehaviour {
        [Serializable]
        private struct PopupType {
            public int Id;
            public ScreenSpacePopupView Proto;
        }

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private PopupType[] popupTypes;

        private readonly IDictionary<int, ScreenSpacePopupView> popupProtos =
            new Dictionary<int, ScreenSpacePopupView>(4);

        public ScreenSpacePopupView Get(int popupType) {
            ScreenSpacePopupView popupProto = popupProtos[popupType];
            int poolId = popupProto.GetComponent<PoolableView>().PoolId;
            return poolStorage.Take(poolId)
                .GetComponent<ScreenSpacePopupView>();
        }

        public void Show(ScreenSpacePopupView popup) {
            bool isWorldPositionStays = false;
            popup.transform.SetParent(transform, isWorldPositionStays);
            var showable = popup.GetComponent<ShowableView>();
            showable.OnHide += OnPopupHide;
            showable.Show();
        }

        private void OnPopupHide(ShowableView showable) {
            poolStorage.Return(showable.GetComponent<PoolableView>());
        }

        private void Awake() {
            for (int i = 0; i < popupTypes.Length; ++i) {
                PopupType popupType = popupTypes[i];
                popupProtos.Add(popupType.Id, popupType.Proto);
            }
        }
    }
}
