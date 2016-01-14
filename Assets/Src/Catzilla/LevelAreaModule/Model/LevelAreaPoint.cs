using UnityEngine;
using System.Collections;

namespace Catzilla.LevelAreaModule.Model {
    public struct LevelAreaPoint {
        public int X {get; private set;}
        public int Z {get; private set;}

        public LevelAreaPoint(int x, int z) {
            X = x;
            Z = z;
        }

        public override bool Equals(object obj) {
            if (obj is LevelAreaPoint) {
                LevelAreaPoint rhs = (LevelAreaPoint) obj;
                return X == rhs.X && Z == rhs.Z;
            }

            return false;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 29 + X.GetHashCode();
                hash = hash * 29 + Z.GetHashCode();
                return hash;
            }
        }

        public override string ToString() {
            return string.Format("x={0}; z={1}", X, Z);
        }
    }
}
