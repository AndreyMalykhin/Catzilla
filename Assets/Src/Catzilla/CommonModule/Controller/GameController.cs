using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.CommonModule.Controller {
    public class GameController {
        [Inject]
        private Game game;

        [Inject]
        private TutorialScreenView tutorialScreen;

        [Inject]
        private CoroutineManagerView coroutineManager;

        public void OnLevelGenerate(Evt evt) {
            game.Pause();
        }

        public void OnLevelStartScreenHide(ShowableView showable) {
            if (tutorialScreen.IsOpened) {
                return;
            }

            game.Resume();
        }

        public void OnTutorialPreShow(ShowableView showable) {
            game.Pause();
        }

        public void OnTutorialHide(ShowableView showable) {
            game.Resume();
        }

        public void OnGameOverScreenShow(ShowableView showable) {
            game.Pause();
        }

        public void OnGameOverScreenHide(ShowableView showable) {
            game.Resume();
        }
    }
}
