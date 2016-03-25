using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class BtnView: MonoBehaviour {
        [Inject]
        public EventBus EventBus {get; set;}

        public AudioClip ClickSound;
        public AudioSource AudioSource;

        [PostInject]
        public void OnConstruct() {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick() {
            EventBus.Fire((int) Events.BtnClick, new Evt(this));
        }
    }
}
