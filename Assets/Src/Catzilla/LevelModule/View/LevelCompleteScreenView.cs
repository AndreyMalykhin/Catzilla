using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelModule.View {
    public class LevelCompleteScreenView: MonoBehaviour {
        public enum Event {Show}

        [Inject]
        public EventBus EventBus {get; set;}

        public Text Msg;
        public AudioClip ShowSound;
        public AudioSource AudioSource;
        public Action OnHide {get; set;}

        [SerializeField]
        private float duration = 2f;

        private Canvas canvas;

        [PostInject]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
            Invoke("Hide", duration);
            EventBus.Fire(Event.Show, new Evt(this));
        }

        public void Hide() {
            canvas.enabled = false;

            if (OnHide != null) {
                OnHide();
            }
        }
    }
}
