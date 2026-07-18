using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2026.Scales
{
    [RequireComponent(typeof(WeighingObjectCollisionTracker))]
    public class WeighingObject : MonoBehaviour
    {
        [SerializeField] private float _weight = 1;
        [SerializeField] private WeighingObjectCollisionTracker _collisionTracker;

        public float Weight => _weight;

        public IReadOnlyCollection<WeighingObject> GetStackRecursively() => _collisionTracker.GetStackOfWeighingObjects();
    }
}