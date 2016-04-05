using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Model {
    public class Game {
        public bool IsPaused {get {return audioManager.IsPaused;}}

        [Inject("LevelScene")]
        private string levelScene;

        [Inject("EmptyScene")]
        private string emptyScene;

        [Inject("LevelTag")]
        private string levelTag;

        [Inject]
        private EventBus eventBus;

        [Inject]
        private AudioManager audioManager;

        [Inject]
        private DiContainer diContainer;

        [Inject]
        private CoroutineManagerView coroutineManager;

        public void Pause() {
            // DebugUtils.Log("Game.Pause()");
            Time.timeScale = 0f;
            audioManager.IsPaused = true;
        }

        public void Resume() {
            // DebugUtils.Log("Game.Resume()");
            Time.timeScale = 1f;
            audioManager.IsPaused = false;
        }

        public void LoadLevel() {
            // DebugUtils.Log("Game.LoadLevel()");
            eventBus.Fire((int) Events.GamePreLevelLoad, new Evt(this));
            coroutineManager.Run(DoLoadLevel());
        }

        public void UnloadLevel() {
            eventBus.Fire((int) Events.GamePreLevelUnload, new Evt(this));
            SceneManager.LoadScene(emptyScene);
            eventBus.Fire((int) Events.GameLevelUnload, new Evt(this));
        }

        private IEnumerator DoLoadLevel() {
            SceneManager.LoadScene(levelScene);
            yield return null;
            diContainer.InjectGameObject(GameObject.FindWithTag(levelTag));
            eventBus.Fire((int) Events.GameLevelLoad, new Evt(this));
        }
    }
}
