using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;

namespace Catzilla.CommonModule.Controller {
    public class GameController {
        [Inject]
        private Game game;

        public void OnLevelStartScreenShow(ShowableView showable) {
            game.Pause();
        }

        public void OnLevelStartScreenHide(ShowableView showable) {
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
