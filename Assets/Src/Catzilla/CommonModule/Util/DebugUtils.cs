using System.Diagnostics;

namespace Catzilla.CommonModule.Util {
    public static class DebugUtils {
        public static void Log(string msg, params object[] args) {
            if (!UnityEngine.Debug.isDebugBuild) {
                return;
            }

            UnityEngine.Debug.LogFormat(msg, args);
        }

        public static void Assert(bool condition) {
            if (!UnityEngine.Debug.isDebugBuild) {
                return;
            }

            Debug.Assert(condition);
        }

        public static string TicksToMilliseconds(Stopwatch stopwatch) {
            return (stopwatch.ElapsedTicks / (double) Stopwatch.Frequency *
                1000).ToString();
        }
    }
}
