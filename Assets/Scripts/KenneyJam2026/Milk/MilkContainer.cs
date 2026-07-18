using KenneyJam2026.Scales;
using UnityEngine;

namespace KenneyJam2026.Milk
{
    [RequireComponent(typeof(WeighingObject))]
    public class MilkContainer : MonoBehaviour
    {
        [SerializeField] private float _capacity = 1;
        [SerializeField] private float _currentCharge = 1;

        public float CurrentCharge => _currentCharge;
    }
}