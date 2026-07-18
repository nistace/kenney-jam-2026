using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class ScaleRatioDampener : MonoBehaviour
    {
        [SerializeField] private Scale _scale;
        [SerializeField] private float _damp = .2f;

        public float DampedValue { get; private set; }
        private float _velocity;

        private void Update()
        {
            DampedValue = Mathf.SmoothDamp(DampedValue, _scale.TiltRatio, ref _velocity, _damp);
        }
    }
}