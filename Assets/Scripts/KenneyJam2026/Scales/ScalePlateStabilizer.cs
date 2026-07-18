using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class ScalePlateStabilizer : MonoBehaviour
    {
        [SerializeField] private Transform _referentTransform;
        [SerializeField] private float _stableValue = 20;
        [SerializeField] private float _stabilizerCoefficient = 1;

        private void FixedUpdate()
        {
            transform.localRotation = Quaternion.Euler(0, 0, _referentTransform.localRotation.eulerAngles.x * _stabilizerCoefficient + _stableValue);
        }
    }
}