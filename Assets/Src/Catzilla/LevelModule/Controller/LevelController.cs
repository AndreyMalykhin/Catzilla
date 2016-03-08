using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
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

        [Inject("PlayStopwatch")]
        public Stopwatch PlayStopwatch {get; set;}

        private LevelView level;

        public void OnViewConstruct(Evt evt) {
            level = (LevelView) evt.Source;
            PlayerState playerState = PlayerStateStorage.Get();
            var msg = Translator.Translate(
                "LevelStartScreen.Level", playerState.Level + 1);
            LevelStartScreen.Msg.text = msg;
            var showable = LevelStartScreen.GetComponent<ShowableView>();
            showable.OnShow = OnStartScreenShow;
            showable.Show();
        }

        private void OnLevelGenerate() {
            MainCamera.gameObject.SetActive(false);
            var showable = LevelStartScreen.GetComponent<ShowableView>();
            showable.OnHide = OnStartScreenHide;
            showable.Hide();
        }

        private void OnStartScreenShow(ShowableView showable) {
            Game.Pause();
            PlayerState playerState = PlayerStateStorage.Get();
            LevelGenerator.NewLevel(playerState.Level, level, OnLevelGenerate);
        }

        private void OnStartScreenHide(ShowableView showable) {
            // DebugUtils.Log(
            //     "LevelController.OnStartScreenHide(); {0}", DateTime.Now);
            PlayStopwatch.Reset();
            PlayStopwatch.Start();
            Game.Resume();
        }
    }
}
