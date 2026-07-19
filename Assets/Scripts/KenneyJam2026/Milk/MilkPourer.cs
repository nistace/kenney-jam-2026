using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class MilkPourer : MonoBehaviour
    {
        [SerializeField] private MilkContainer _container;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private ParticleSystem _splashParticles;
        [SerializeField] private Transform _pourOrigin;
        [SerializeField] private AnimationCurve _tiltCurvePerContainerCharge;
        [SerializeField] private float _flowRate = .1f;
        [SerializeField] private Vector3 _pourDirection = Vector3.down;
        [SerializeField] private float _maxPourDistance = 1;
        [SerializeField] private LayerMask _pourMask;

        private static readonly RaycastHit[] NonAllocHits = new RaycastHit[5];

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

            if (!pouring) return;

            var removedQuantity = _container.Remove(_flowRate * Time.deltaTime);

            if (removedQuantity > 0)
            {
                ResolvePouring(removedQuantity);
            }
        }

        private void ResolvePouring(float pouredAmount)
        {
            var hits = Physics.RaycastNonAlloc(new Ray(_pourOrigin.position, _pourDirection), NonAllocHits, _maxPourDistance, _pourMask);

            var bestHitDistance = 2 * _maxPourDistance;
            RaycastHit bestHit = default;

            for (var i = 0; i < hits; i++)
            {
                var hit = NonAllocHits[i];

                if (hit.collider.transform.IsChildOf(transform))
                {
                    continue;
                }

                if (hit.distance > bestHitDistance)
                {
                    continue;
                }

                bestHitDistance = hit.distance;
                bestHit = hit;
            }

            if (!(bestHitDistance < _maxPourDistance))
            {
                return;
            }

            if (bestHit.collider.TryGetComponent(out MilkPouringReceiver receiver))
            {
                if (receiver.Feed(pouredAmount))
                {
                    return;
                }
            }

            _splashParticles.transform.position = bestHit.point;
            _splashParticles.transform.up = bestHit.normal;
            _splashParticles.Play();
        }

        private void OnDrawGizmos()
        {
            if (_pourOrigin == null) return;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(_pourOrigin.position, _pourOrigin.position + _pourDirection.normalized * _maxPourDistance);
        }
    }
}