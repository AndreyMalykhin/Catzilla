using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelObjectModule.Controller {
    public class BonusController {
        [Inject]
        private BonusSpawnResolver bonusSpawnResolver;

        public void OnViewDestroy(Evt evt) {
            // DebugUtils.Log("BonusController.OnViewDestroy()");
            --bonusSpawnResolver.ActiveBonusObjects;
        }
    }
}
