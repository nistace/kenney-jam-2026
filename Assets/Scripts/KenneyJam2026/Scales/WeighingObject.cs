using System;
using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2026.Scales
{
    [RequireComponent(typeof(WeighingObjectCollisionTracker))]
    public class WeighingObject : MonoBehaviour
    {
        [SerializeField] private float _weight = 1;
        [SerializeField] private WeighingObjectCollisionTracker _collisionTracker;

        private float _additionalTotalWeight;

        public float TotalWeight => _weight + AdditionalDynamicWeight;


        public float AdditionalDynamicWeight
        {
            get => _additionalTotalWeight;
            set
            {
                if (Mathf.Approximately(_additionalTotalWeight, value)) return;

                _additionalTotalWeight = value;
                OnWeightChanged?.Invoke();
            }
        }

        public event Action OnWeightChanged;

        public IReadOnlyCollection<WeighingObject> GetStackRecursively() => _collisionTracker.GetStackOfWeighingObjects();
    }
}