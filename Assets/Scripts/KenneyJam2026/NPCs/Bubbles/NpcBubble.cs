using UnityEngine;
using Random = UnityEngine.Random;

namespace KenneyJam2026.NPCs.Bubbles
{
    public class NpcBubble : MonoBehaviour
    {
        [SerializeField] private NpcBubbleConfig _config;
        [SerializeField] private Transform _orientationOrigin;

        private Transform TargetPosition { get; set; }

        public Transform OrientationOrigin
        {
            get => _orientationOrigin;
            set => _orientationOrigin = value;
        }

        private float RandomOffset { get; set; }

        private Vector3 _currentVelocity;
        private float _jiggleAngle;

        private enum GrowState
        {
            Appearing = 0,
            Visible = 1,
            Disappearing = 2,
            Disappeared = 3
        }

        private float _growTime;
        private GrowState _growState = GrowState.Disappeared;

        private void Start()
        {
            RandomOffset = Random.Range(0, 10f);
        }

        public void Appear(Transform bubblePlaceholder)
        {
            TargetPosition = bubblePlaceholder;
            _currentVelocity = Vector3.zero;

            if (_growState != GrowState.Visible)
            {
                _growState = GrowState.Appearing;
                _growTime = 0;
            }

            if (TargetPosition)
            {
                transform.position = TargetPosition.position;
            }

            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            if (_growState is not (GrowState.Disappeared or GrowState.Disappearing))
            {
                _growState = GrowState.Disappearing;
                _growTime = 0;
            }

            TargetPosition = null;
        }

        private void Update()
        {
            UpdateScale();
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var expectedAngle = Vector3.SignedAngle(Vector3.up, transform.position - OrientationOrigin.position, Vector3.forward);
            var targetAngle = expectedAngle + _config.JiggleAngleCurve.Evaluate(RandomOffset + Time.time);

            _jiggleAngle = Mathf.MoveTowardsAngle(_jiggleAngle, targetAngle, _config.MaxRotationPerSecond * Time.deltaTime);

            transform.localRotation = Quaternion.Euler(0, 0, _jiggleAngle);
        }

        private void UpdateScale()
        {
            if (_growState == GrowState.Visible)
            {
                return;
            }

            _growTime += Time.deltaTime;

            var growCurve = _growState == GrowState.Appearing ? _config.AppearScaleCurve : _config.DisappearScaleCurve;
            var transitionDuration = growCurve.keys[^1].time;

            if (transitionDuration > _growTime)
            {
                transform.localScale = _growState == GrowState.Appearing ? Vector3.one : Vector3.zero;

                if (_growState == GrowState.Disappearing)
                {
                    gameObject.SetActive(false);
                }

                _growState = _growState == GrowState.Appearing ? GrowState.Visible : GrowState.Disappeared;
                _growTime = 0;

                return;
            }

            var scale = growCurve.Evaluate(_growTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void UpdatePosition()
        {
            if (TargetPosition)
            {
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition.position, ref _currentVelocity, _config.Damp);
            }
        }
    }
}