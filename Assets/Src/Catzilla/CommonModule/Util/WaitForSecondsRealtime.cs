using UnityEngine;

namespace Catzilla.CommonModule.Util {
    public class WaitForSecondsRealtime: CustomYieldInstruction {
        public override bool keepWaiting {
            get {
                return Time.realtimeSinceStartup < waitTime;
            }
        }

        private readonly float waitTime;

        public WaitForSecondsRealtime(float duration) {
            waitTime = Time.realtimeSinceStartup + duration;
        }
    }
}
