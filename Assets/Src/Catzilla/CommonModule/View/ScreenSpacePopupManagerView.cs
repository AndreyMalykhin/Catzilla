using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ScreenSpacePopupManagerView: MonoBehaviour {
        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private ScreenSpacePopupView popupProto;

        public ScreenSpacePopupView Get() {
            int poolId = popupProto.GetComponent<PoolableView>().PoolId;
            return poolStorage.Take(poolId)
                .GetComponent<ScreenSpacePopupView>();
        }

        public void Show(ScreenSpacePopupView popup) {
            bool isWorldPositionStays = false;
            popup.transform.SetParent(transform, isWorldPositionStays);
            var showable = popup.GetComponent<ShowableView>();
            showable.OnHide = OnPopupHide;
            showable.Show();
        }

        private void OnPopupHide(ShowableView showable) {
            poolStorage.Return(showable.GetComponent<PoolableView>());
        }
    }
}
