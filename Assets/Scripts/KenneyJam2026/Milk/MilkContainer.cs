using System;
using KenneyJam2026.Scales;
using UnityEngine;

namespace KenneyJam2026.Milk
{
    [RequireComponent(typeof(WeighingObject))]
    public class MilkContainer : MonoBehaviour
    {
        [SerializeField] private WeighingObject _weighingObject;
        [SerializeField] private float _capacity = 1;
        [SerializeField] private float _currentCharge = 1;

        public float CurrentCharge => _currentCharge;
        public float CurrentChargeRatio => _currentCharge / _capacity;

        public event Action OnChargeChanged;

        private void Start()
        {
            _weighingObject.AdditionalDynamicWeight = _currentCharge;
        }

        public void Add(float value) => Change(value);
        public void Remove(float value) => Change(-value);

        private void Change(float delta)
        {
            if (Mathf.Approximately(delta, 0))
            {
                return;
            }

            _currentCharge = Mathf.Clamp(_currentCharge + delta, 0, _capacity);
            _weighingObject.AdditionalDynamicWeight = _currentCharge;
            OnChargeChanged?.Invoke();
        }
    }
}