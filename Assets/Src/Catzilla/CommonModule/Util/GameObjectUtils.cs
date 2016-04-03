using UnityEngine;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    public static class GameObjectUtils {
        public static void SetActive(GameObject obj, bool isActive,
            DeactivatableView deactivatable = null) {
            if (deactivatable == null) {
                obj.SetActive(isActive);
                return;
            }

            deactivatable.IsActive = isActive;
        }
    }
}
