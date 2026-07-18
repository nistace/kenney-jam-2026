using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class Scale : MonoBehaviour
    {
        [SerializeField] private ScalePlate _leftPlate;
        [SerializeField] private ScalePlate _rightPlate;
        [SerializeField] private float _sensitivityWeight = 1;

        private float _leftPlateWeight;
        private float _rightPlateWeight;
        public float TiltRatio { get; private set; }

        private void Update()
        {
            _leftPlateWeight = _leftPlate.GetWeight();
            _rightPlateWeight = _rightPlate.GetWeight();

            TiltRatio = Mathf.Clamp((_leftPlateWeight - _rightPlateWeight) / _sensitivityWeight, -1f, 1f);
        }
    }
}