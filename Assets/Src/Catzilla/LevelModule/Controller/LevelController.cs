using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public LevelStartScreenView LevelStartScreen {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        [Inject]
        public Game Game {get; set;}

        private LevelView level;

        public void OnViewConstruct(Evt evt) {
            level = (LevelView) evt.Source;
            PlayerState playerState = PlayerStateStorage.Get();
            var msg = Translator.Translate(
                "LevelStartScreen.Level", playerState.Level + 1);
            LevelStartScreen.Msg.text = msg;
            LevelStartScreen.Show();
            Game.Pause();
            LevelGenerator.NewLevel(playerState.Level, level, OnLevelGenerate);
        }

        private void OnLevelGenerate() {
            MainCamera.gameObject.SetActive(false);
            LevelStartScreen.Hide(OnStartScreenHide);
        }

        private void OnStartScreenHide() {
            Game.Resume();
            DebugUtils.Log("start {0}", DateTime.Now);
        }
    }
}
