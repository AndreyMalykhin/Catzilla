using UnityEngine;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public class MeshesComparer: IEqualityComparer<Mesh[]> {
        public bool Equals(Mesh[] lhs, Mesh[] rhs) {
            if (lhs.Length != rhs.Length) {
                return false;
            }

            for (int i = 0; i < lhs.Length; ++i) {
                if (lhs[i] != rhs[i]) {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(Mesh[] meshes) {
            int hash = 17;

            for (int i = 0; i < meshes.Length; ++i) {
                unchecked {
                    hash *= 23 + meshes[i].GetHashCode();
                }
            }

            return hash;
        }
    }
}
