using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelAreaModule.Controller {
    public class LevelAreaController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        private LevelView level;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            LevelGenerator.NewArea(level);
        }
    }
}
