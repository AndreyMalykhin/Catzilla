using UnityEngine;
using System.Collections;

namespace Catzilla.LevelObjectModule.View {
    public class PlayerCameraView: strange.extensions.mediation.impl.View {
        private void LateUpdate() {
            transform.position =
                new Vector3(0f, transform.position.y, transform.position.z);
        }
    }
}
