using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class ScaleRotator : MonoBehaviour
    {
        private enum RotationAxis
        {
            X = 0,
            Y = 1,
            Z = 2
        }

        [SerializeField] private ScaleRatioDampener _scale;
        [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Z;
        [SerializeField] private float _maxTilt = 25;

        private void Update()
        {
            var angle = _scale.DampedValue * _maxTilt;

            transform.localRotation = Quaternion.Euler(_rotationAxis == RotationAxis.X ? angle : 0, _rotationAxis == RotationAxis.Y ? angle : 0, _rotationAxis == RotationAxis.Z ? angle : 0);
        }
    }
}