using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class CombinedView: MonoBehaviour {
        [SerializeField]
        private MeshFilter[] parts;

        [SerializeField]
        private MeshFilter output;

        [SerializeField]
        private bool keepParts;

        private static readonly IDictionary<Mesh[], Mesh> cachedOutputMeshes =
            new Dictionary<Mesh[], Mesh>(64, new MeshesComparer());

        private void Awake() {
            Combine();
        }

        private void Combine() {
            Mesh outputMesh = null;
            int partsCount = parts.Length;
            var partMeshes = new Mesh[partsCount];

            for (int i = 0; i < partsCount; ++i) {
                MeshFilter part = parts[i];
                partMeshes[i] = part.sharedMesh;

                if (keepParts) {
                    part.gameObject.SetActive(false);
                } else {
                    Destroy(part.gameObject);
                }
            }

            if (!cachedOutputMeshes.TryGetValue(partMeshes, out outputMesh)) {
                outputMesh = DoCombine(parts);
                cachedOutputMeshes.Add(partMeshes, outputMesh);
            }

            output.sharedMesh = outputMesh;
            parts = null;
        }

        private Mesh DoCombine(MeshFilter[] parts) {
            Vector3 outputPosition = output.transform.position;
            Quaternion outputRotation = output.transform.rotation;
            int partsCount = parts.Length;
            CombineInstance[] combine = new CombineInstance[partsCount];
            output.transform.position = Vector3.zero;
            output.transform.rotation = Quaternion.identity;

            for (int i = 0; i < partsCount; ++i) {
                MeshFilter part = parts[i];
                combine[i].mesh = part.sharedMesh;
                combine[i].transform =
                    part.transform.localToWorldMatrix;
            }

            output.transform.position = outputPosition;
            output.transform.rotation = outputRotation;
            var outputMesh = new Mesh();
            outputMesh.CombineMeshes(combine, true, true);
            return outputMesh;
        }
    }
}
