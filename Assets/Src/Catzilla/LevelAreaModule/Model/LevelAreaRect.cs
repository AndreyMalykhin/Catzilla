using UnityEngine;
using System.Collections;

namespace Catzilla.LevelAreaModule.Model {
    public struct LevelAreaRect {
        public LevelAreaPoint Start {get; private set;}
        public int Width {get; private set;}
        public int Depth {get; private set;}

        public LevelAreaRect(LevelAreaPoint start, int width, int depth) {
            Start = start;
            Width = width;
            Depth = depth;
        }
    }
}
