using UnityEngine;
using System.Collections;
using Catzilla.GameOverMenuModule.View;
using Catzilla.PlayerModule.Model;

namespace Catzilla.PlayerModule.Controller {
    public class PlayerStateStorageController {
        [Inject]
        public GameOverMenuView GameOverMenu {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        public void OnSave() {
            GameOverMenu.AvailableResurrectionsCount =
                PlayerStateStorage.Get().AvailableResurrectionsCount;
        }
    }
}
