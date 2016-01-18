using UnityEngine;
using System.Collections;
using Catzilla.AppModule.Model;

namespace Catzilla.LevelModule.Controller {
    public class LevelCompleteScreenController {
        [Inject]
        public Game Game {get; set;}

        public void OnHide() {
            Game.LoadLevel();
        }
    }
}
