using UnityEngine;

namespace KenneyJam2026.Scales
{
    [RequireComponent(typeof(WeighingObjectCollisionTracker))]
    public class ScalePlate : MonoBehaviour
    {
        [SerializeField] private WeighingObjectCollisionTracker _collisionTracker;

        public float GetWeight()
        {
            var totalWeight = 0f;

            foreach (var weighingObject in _collisionTracker.GetStackOfWeighingObjects())
            {
                totalWeight += weighingObject.TotalWeight;
            }

            return totalWeight;
        }
    }
}