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
        private LevelGenerator levelGenerator;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private LevelStartScreenView levelStartScreen;

        [Inject]
        private Translator translator;

        [Inject]
        private TutorialScreenView tutorialScreen;

        private LevelView level;

        public void OnViewConstruct(Evt evt) {
            level = (LevelView) evt.Source;
            PlayerState playerState = playerStateStorage.Get();
            var msg = translator.Translate(
                "LevelStartScreen.Level", playerState.Level + 1);
            levelStartScreen.Msg.text = msg;
            var showable = levelStartScreen.GetComponent<ShowableView>();
            showable.OnShow += OnStartScreenShow;
            showable.Show();
        }

        private void OnStartScreenShow(ShowableView showable) {
            showable.OnShow -= OnStartScreenShow;
            PlayerState playerState = playerStateStorage.Get();
            levelGenerator.NewLevel(playerState.Level, level, OnLevelGenerate);
        }

        private void OnLevelGenerate() {
            var showable = levelStartScreen.GetComponent<ShowableView>();
            showable.OnHide += OnStartScreenHide;
            showable.Hide();
        }

        private void OnStartScreenHide(ShowableView showable) {
            showable.OnHide -= OnStartScreenHide;
            PlayerState playerState = playerStateStorage.Get();

            if (playerState.Level == 0 && !playerState.WasTutorialShown) {
                var tutorialShowable = tutorialScreen.GetComponent<ShowableView>();
                tutorialShowable.OnShow += OnTutorialShow;
                tutorialShowable.Show();
                playerState.WasTutorialShown = true;
            }
        }

        private void OnTutorialShow(ShowableView showable) {
            showable.OnShow -= OnTutorialShow;
            var tutorialScreen = showable.GetComponent<TutorialScreenView>();
            tutorialScreen.OnClose += OnTutorialClose;
            tutorialScreen.Open();
        }

        private void OnTutorialClose(TutorialScreenView tutorialScreen) {
            tutorialScreen.OnClose -= OnTutorialClose;
            tutorialScreen.GetComponent<ShowableView>().Hide();
        }
    }
}
