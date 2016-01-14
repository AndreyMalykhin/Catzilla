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

        private Level level;
        private LevelView levelView;

        public void Load(int levelIndex) {
            Debug.Log("LevelController.Load()");
            level = LevelGenerator.Generate(levelIndex);
            SceneManager.LoadScene(LevelScene);
        }

        public void OnViewReady(IEvent evt) {
            levelView = (LevelView) evt.data;
            levelView.Init(level);
        }

        public void OnAreaTriggerEnter(IEvent evt) {
            if (((Collider) evt.data).CompareTag(PlayerTag)) {
                levelView.AddArea(LevelGenerator.GenerateArea(level.Index));
            }
        }
    }
}
