using UnityEngine;

namespace Catzilla.CommonModule.Util {
    public class WaitForSecondsRealtime: CustomYieldInstruction {
        public override bool keepWaiting {
            get {
                return Time.realtimeSinceStartup < stopWaitingTime;
            }
        }

        private float stopWaitingTime;
        private float duration;

        public WaitForSecondsRealtime(float duration) {
            this.duration = duration;
            Restart();
        }

        public void Restart() {
            stopWaitingTime = Time.realtimeSinceStartup + duration;
        }
    }
}
