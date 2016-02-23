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
            popup.OnHide = OnPopupHide;
            bool isWorldPositionStays = false;
            popup.transform.SetParent(transform, isWorldPositionStays);
            popup.Show();
        }

        private void OnPopupHide(ScreenSpacePopupView popup) {
            poolStorage.Return(popup.GetComponent<PoolableView>());
        }
    }
}
