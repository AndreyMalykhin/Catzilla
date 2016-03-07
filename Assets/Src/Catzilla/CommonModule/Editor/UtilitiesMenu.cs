using UnityEngine;
using UnityEditor;

namespace Catzilla.CommonModule.Editor {
    public static class MenuItems {
        [MenuItem("Tools/Player prefs/Clear")]
        private static void NewMenuOption() {
            PlayerPrefs.DeleteAll();
        }
    }
}
