using UnityEngine;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public class Vector3Comparer: IEqualityComparer<Vector3> {
        public bool Equals(Vector3 lhs, Vector3 rhs) {
            return lhs == rhs;
        }

        public int GetHashCode(Vector3 vector) {
            return vector.GetHashCode();
        }
    }
}
