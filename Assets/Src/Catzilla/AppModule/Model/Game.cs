using UnityEngine;
using UnityEngine.SceneManagement;

namespace Catzilla.AppModule.Model {
    public class Game {
        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        public void Pause() {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        public void Resume() {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        public void Exit() {
            Application.Quit();
        }

        public void LoadLevel() {
            SceneManager.LoadScene(LevelScene);
        }
    }
}
