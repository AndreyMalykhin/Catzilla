using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class BonusController {
        [Inject]
        private LevelGenerator levelGenerator;

        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var bonus = (BonusView) evt.Source;
            bonus.Take();
        }

        public void OnViewDestroy(Evt evt) {
            // DebugUtils.Log("BonusController.OnViewDestroy()");
            --levelGenerator.ActiveBonusObjects;
        }
    }
}
