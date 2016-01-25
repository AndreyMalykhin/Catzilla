using UnityEngine;
using UnityEngine.SceneManagement;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Model {
    public class Game {
        public enum Event {LevelLoad}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void Pause() {
            Debug.Log("Game.Pause()");
            Time.timeScale = 0f;
            AudioManager.IsPaused = true;
        }

        public void Resume() {
            Debug.Log("Game.Resume()");
            Time.timeScale = 1f;
            AudioManager.IsPaused = false;
        }

        public void Exit() {
            Application.Quit();
        }

        public void LoadLevel() {
            Debug.Log("Game.LoadLevel()");
            SceneManager.LoadScene(LevelScene);
            EventBus.Dispatch(Event.LevelLoad);
        }
    }
}
