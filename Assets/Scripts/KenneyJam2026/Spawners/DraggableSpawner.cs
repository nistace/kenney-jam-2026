using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Spawners
{
    public class DraggableSpawner : MonoBehaviour
    {
        [SerializeField] private DraggableObject _prefab;

        public DraggableObject Spawn() => Instantiate(_prefab, transform.position, transform.rotation);

        private void OnDrawGizmos()
        {
            if (!_prefab) return;
            Gizmos.color = Color.green;

            Gizmos.matrix = transform.localToWorldMatrix;
            foreach (var meshFilter in _prefab.GetComponentsInChildren<MeshFilter>())
            {
                Gizmos.DrawMesh(meshFilter.sharedMesh, meshFilter.transform.localPosition, meshFilter.transform.localRotation, meshFilter.transform.localScale);
            }
        }
    }
}