using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class MilkPourer : MonoBehaviour
    {
        [SerializeField] private MilkContainer _container;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Transform _pourOrigin;
        [SerializeField] private AnimationCurve _tiltCurvePerContainerCharge;
        [SerializeField] private float _flowRate = .1f;
        
        public Vector3 GetRequiredUpToPour() => GetRequiredUpToPourWithRatio(Mathf.Max(0f, _container.CurrentChargeRatio));
        public Vector3 GetRequiredUpToPourAtLowerCharge() => GetRequiredUpToPourWithRatio(Mathf.Max(0f, _container.CurrentChargeRatio - .1f));

        private Vector3 GetRequiredUpToPourWithRatio(float ratio)
        {
            var tilt = _tiltCurvePerContainerCharge.Evaluate(ratio);
            return Vector3.SlerpUnclamped(Vector3.up, Vector3.Cross(transform.forward, Vector3.up), tilt);
        }

        private void Update()
        {
            var angleWithUp = Mathf.Abs(Vector3.SignedAngle(transform.up, Vector3.up, transform.forward));
            var requiredAngleWithUpToPour = Mathf.Abs(Vector3.SignedAngle(GetRequiredUpToPour(), Vector3.up, transform.forward));

            var pouring = _container.CurrentCharge > 0 && angleWithUp >= requiredAngleWithUpToPour;
            if (pouring && !_particles.isPlaying)
            {
                _particles.Play();
            }
            else if (!pouring && _particles.isPlaying)
            {
                _particles.Stop();
            }

            if (pouring)
            {
                _container.Remove(_flowRate * Time.deltaTime);
            }
        }
    }
}