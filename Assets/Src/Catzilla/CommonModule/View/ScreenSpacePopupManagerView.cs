using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ScreenSpacePopupManagerView: PopupManagerView {
        protected override void OnPreShow(PopupView popup) {
            bool isWorldPositionStays = false;
            Transform popupTransform = popup.transform;
            popupTransform.SetParent(transform, isWorldPositionStays);
            popupTransform.SetAsLastSibling();
        }
    }
}
