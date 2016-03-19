using UnityEngine;
using UnityEngine.Analytics;
using System.Text;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public static class AnalyticsUtils {
        private static readonly StringBuilder stringBuilder =
            new StringBuilder(64);
        private static readonly IDictionary<string, object> nextEventParams =
            new Dictionary<string, object>(8);

        public static void AddEventParam(string name, object value) {
            if (Debug.isDebugBuild) {
                return;
            }

            nextEventParams[name] = value;
        }

        public static void AddCategorizedEventParam(string name, object value) {
            if (Debug.isDebugBuild) {
                return;
            }

            nextEventParams[name] =
                stringBuilder.Append(name).Append(value).ToString();
            stringBuilder.Length = 0;
        }

        public static void LogEvent(string name) {
            if (Debug.isDebugBuild) {
                return;
            }

            Analytics.CustomEvent(name, nextEventParams);
            nextEventParams.Clear();
        }
    }
}
