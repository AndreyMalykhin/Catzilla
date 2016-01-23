using UnityEngine;
using System.Collections;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class GameController {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        public void OnLevelLoad() {
            PoolStorage.Reset();
        }
    }
}
