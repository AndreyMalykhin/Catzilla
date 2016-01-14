using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelModule.Model {
    public struct Level {
        public int Index {get; private set;}
        public List<LevelArea> Areas {get; private set;}

        public Level(int index, List<LevelArea> areas) {
            Index = index;
            Areas = areas;
        }
    }
}
