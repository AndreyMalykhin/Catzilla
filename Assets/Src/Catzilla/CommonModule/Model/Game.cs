﻿using UnityEngine;
using UnityEngine.SceneManagement;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Model {
    public class Game {
        public enum Event {LevelLoad}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

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
            SceneManager.LoadScene(LevelScene);
            EventBus.Fire(Event.LevelLoad, new Evt(this));
        }
    }
}
