using UnityEngine;
using System;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [Serializable]
    public class ObjectProtoInfo {
        public Material[] AvailableMaterials {get {return availableMaterials;}}
        public LevelObjectView View {get {return view;}}

        [SerializeField]
        private LevelObjectView view;

        [SerializeField]
        private Material[] availableMaterials;
    }
}

