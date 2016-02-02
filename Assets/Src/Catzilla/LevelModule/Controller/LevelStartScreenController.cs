using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelStartScreenController {
        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        private LevelView level;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnHide(Evt evt) {
            level.gameObject.SetActive(true);
            MainCamera.gameObject.SetActive(false);
        }
    }
}
