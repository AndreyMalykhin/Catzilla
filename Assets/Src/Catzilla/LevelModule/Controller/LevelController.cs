using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelAreaModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        private LevelView level;
        private int levelIndex;

        public void Load(int levelIndex) {
            Debug.Log("LevelController.Load()");
            this.levelIndex = levelIndex;
            SceneManager.LoadScene(LevelScene);
        }

        public void OnViewReady(IEvent evt) {
            level = (LevelView) evt.data;
            level.Index = levelIndex;
            LevelGenerator.NewLevel(level);
        }

        public void OnAreaTriggerEnter(IEvent evt) {
            if (((Collider) evt.data).CompareTag(PlayerTag)) {
                 LevelGenerator.NewArea(level);
            }
        }
    }
}
