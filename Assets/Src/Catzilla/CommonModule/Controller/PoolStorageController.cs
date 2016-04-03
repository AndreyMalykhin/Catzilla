using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Controller {
    public class PoolStorageController {
        [Inject]
        private PoolStorageView poolStorage;

        public void OnLevelLoad(Evt evt) {
            poolStorage.Cleanup();
            poolStorage.Refill();
            GC.Collect();
        }
    }
}
