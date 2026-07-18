using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class WeighingObjectCollisionTracker : MonoBehaviour
    {
        [SerializeField] private Transform _positionReference;

        private readonly HashSet<Collider> _touchingColliders = new();
        private readonly Dictionary<Collider, WeighingObject> _touchingCollidersWeighingObjects = new();
        private readonly HashSet<WeighingObject> _weighingObjectsTouchingPlateRecursive = new();

        private void OnCollisionEnter(Collision other)
        {
            if (!_touchingColliders.Add(other.collider)) return;
            if (_touchingCollidersWeighingObjects.ContainsKey(other.collider)) return;
            if (!other.collider.gameObject.TryGetComponent(out WeighingObject weighingObject)) return;

            _touchingCollidersWeighingObjects.Add(other.collider, weighingObject);
        }

        private void OnCollisionExit(Collision other)
        {
            _touchingColliders.Remove(other.collider);
        }

        public IReadOnlyCollection<WeighingObject> GetStackOfWeighingObjects()
        {
            _weighingObjectsTouchingPlateRecursive.Clear();

            foreach (var touchingCollider in _touchingColliders)
            {
                if (!_touchingCollidersWeighingObjects.TryGetValue(touchingCollider, out var weighingObject)) continue;
                if (touchingCollider.transform.position.y < _positionReference.position.y) continue;

                _weighingObjectsTouchingPlateRecursive.Add(weighingObject);
                foreach (var stackedObject in weighingObject.GetStackRecursively())
                {
                    _weighingObjectsTouchingPlateRecursive.Add(stackedObject);
                }
            }

            return _weighingObjectsTouchingPlateRecursive;
        }
    }
}