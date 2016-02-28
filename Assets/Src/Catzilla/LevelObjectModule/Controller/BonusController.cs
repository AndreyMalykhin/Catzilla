using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelObjectModule.Controller {
    public class BonusController {
        [Inject]
        private LevelGenerator levelGenerator;

        public void OnViewDestroy(Evt evt) {
            // DebugUtils.Log("BonusController.OnViewDestroy()");
            --levelGenerator.ActiveBonusObjects;
        }
    }
}
