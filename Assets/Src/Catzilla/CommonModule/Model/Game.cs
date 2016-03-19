using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Model {
    public class Game {
        public enum Event {PreLevelLoad, LevelLoad, LevelUnload}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        [Inject("EmptyScene")]
        public string EmptyScene {get; set;}

        [Inject("LevelTag")]
        public string LevelTag {get; set;}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject]
        public DiContainer Container {get; set;}

        [Inject]
        public CoroutineManagerView CoroutineManager {get; set;}

        public void Pause() {
            // DebugUtils.Log("Game.Pause()");
            Time.timeScale = 0f;
            AudioManager.IsPaused = true;
        }

        public void Resume() {
            // DebugUtils.Log("Game.Resume()");
            Time.timeScale = 1f;
            AudioManager.IsPaused = false;
        }

        public void Exit() {
            Application.Quit();
        }

        public void LoadLevel() {
            // DebugUtils.Log("Game.LoadLevel()");
            EventBus.Fire(Event.PreLevelLoad, new Evt(this));
            CoroutineManager.Run(DoLoadLevel());
        }

        public void UnloadLevel() {
            SceneManager.LoadScene(EmptyScene);
            EventBus.Fire(Event.LevelUnload, new Evt(this));
        }

        private IEnumerator DoLoadLevel() {
            SceneManager.LoadScene(LevelScene);
            yield return null;
            Container.InjectGameObject(GameObject.FindWithTag(LevelTag));
            EventBus.Fire(Event.LevelLoad, new Evt(this));
        }
    }
}
