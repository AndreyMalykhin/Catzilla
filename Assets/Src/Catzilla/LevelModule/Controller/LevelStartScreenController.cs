using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelStartScreenController {
        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        private LevelView level;

        public void OnLevelConstruct(IEvent evt) {
            level = (LevelView) evt.data;
        }

        public void OnHide() {
            level.gameObject.SetActive(true);
            MainCamera.gameObject.SetActive(false);
        }
    }
}
