using UnityEngine;
using UnityEngine.Advertisements;
using System;
using System.Collections;

namespace Catzilla.CommonModule.Util {
    public class Ad {
        private Action onFinish;

        public void Show(Action onFinish) {
            if (!Advertisement.IsReady()) {
                return;
            }

            this.onFinish = onFinish;
            Advertisement.Show(
                null, new ShowOptions{resultCallback = OnShowResult});
        }

        private void OnShowResult(ShowResult result) {
            switch (result) {
                case ShowResult.Finished:
                    onFinish();
                    break;
                case ShowResult.Skipped:
                    break;
                case ShowResult.Failed:
                    break;
            }
        }
    }
}
