using UnityEngine;
using UnityEngine.Advertisements;
using System;
using System.Collections;

namespace Catzilla.CommonModule.Util {
    public class Ad {
        public event Action<Ad> OnView;

        public void Show() {
            if (!Advertisement.IsReady()) {
                return;
            }

            Advertisement.Show(
                null, new ShowOptions{resultCallback = OnShowResult});
        }

        private void OnShowResult(ShowResult result) {
            switch (result) {
                case ShowResult.Finished:
                    if (OnView != null) OnView(this);
                    break;
                case ShowResult.Skipped:
                    break;
                case ShowResult.Failed:
                    break;
            }
        }
    }
}
