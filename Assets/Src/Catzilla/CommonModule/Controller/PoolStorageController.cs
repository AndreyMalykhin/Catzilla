using UnityEngine;
using System;
using System.Collections;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class PoolStorageController {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        public void OnLevelLoad() {
            PoolStorage.Reset();
            GC.Collect();
        }
    }
}
