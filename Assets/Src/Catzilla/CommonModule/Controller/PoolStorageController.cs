using UnityEngine;
using System;
using System.Collections;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Controller {
    public class PoolStorageController {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        public void OnLevelLoad(Evt evt) {
            PoolStorage.Refill();
            GC.Collect();
        }
    }
}
