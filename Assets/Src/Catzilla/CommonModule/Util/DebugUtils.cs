using System.Diagnostics;

namespace Catzilla.CommonModule.Util {
    public static class DebugUtils {
        static DebugUtils() {
            UnityEngine.Assertions.Assert.raiseExceptions = true;
        }

        public static void Log(string msg, params object[] args) {
            if (!UnityEngine.Debug.isDebugBuild) {
                return;
            }

            UnityEngine.Debug.LogFormat(msg, args);
        }

        public static void Assert(bool condition) {
            UnityEngine.Assertions.Assert.IsTrue(condition);
        }

        public static double TicksToMilliseconds(Stopwatch stopwatch) {
            return stopwatch.ElapsedTicks / (double) Stopwatch.Frequency * 1000;
        }
    }
}
