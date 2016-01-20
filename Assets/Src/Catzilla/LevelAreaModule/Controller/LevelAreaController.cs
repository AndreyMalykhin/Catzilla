using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelAreaModule.Controller {
    public class LevelAreaController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        private LevelView level;

        public void OnLevelConstruct(IEvent evt) {
            level = (LevelView) evt.data;
        }

        public void OnTriggerEnter(IEvent evt) {
            var collider = (Collider) evt.data;

            if (collider != null && collider.CompareTag(PlayerTag)) {
                 LevelGenerator.NewArea(level);
            }
        }
    }
}
