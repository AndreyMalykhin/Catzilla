using UnityEngine;
using UnityEngine.SceneManagement;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.CommonModule.Model {
    public class Game {
        public enum Event {LevelLoad}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public void Pause() {
            Debug.Log("Game.Pause()");
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        public void Resume() {
            Debug.Log("Game.Resume()");
            Time.timeScale = 1f;
            AudioListener.pause = false;
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
