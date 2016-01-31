using UnityEngine;

namespace Catzilla.CommonModule.Util {
    public static class DebugUtils {
        public static void Log(string msg, params object[] args) {
            if (!Debug.isDebugBuild) {
                return;
            }

            DebugUtils.Log(msg, args);
        }
    }
}
