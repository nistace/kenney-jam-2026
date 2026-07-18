using System.Collections.Generic;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Player
{
    public class PlayerDragger : MonoBehaviour
    {
        [SerializeField] private Transform _playerLookOrigin;
        [SerializeField] private PlayerInteractor _interactor;
        [SerializeField] private PlayerAimDetector _aimer;
        [SerializeField] private float _stickyCoefficient = 5;
        [SerializeField] private float _dampHardness = 0.2f;
        [SerializeField] private float _maxSpeed = 6;
        [SerializeField] private float _dropForce = 4;
        [SerializeField] private float _defaultLerpDistanceFromPlayer = .5f;

        private IDraggable _draggedObject;
        private Vector3 _draggedObjectAcceleration;
        private LayerMask _previousHitLayerMask;
        private LayerMask _previousRelevantLayerMask;

        private readonly Dictionary<GameObject, PlayerDragAimCatcher> _aimCatchers = new();

        public bool IsDragging => enabled && _draggedObject != null;

        private void Start()
        {
            _interactor.OnInteractionStarted += HandleInteractionStarted;
        }

        private void OnDisable()
        {
            _interactor.OnInteractionEnded -= HandleInteractionEnded;
        }

        private void OnDestroy()
        {
            _interactor.OnInteractionStarted -= HandleInteractionStarted;
        }

        private void HandleInteractionStarted(IInteractable interactable)
        {
            if (interactable is not IDraggable draggable)
            {
                return;
            }

            _draggedObject = draggable;
            _draggedObject.SetOnDraggedLayer(true);
            _draggedObjectAcceleration = Vector3.zero;

            _previousHitLayerMask = _aimer.HitMask;
            _previousRelevantLayerMask = _aimer.RelevantMask;

            _aimer.HitMask = _draggedObject.GetDraggedInteractionLayerMask(_previousHitLayerMask);
            _aimer.RelevantMask = _draggedObject.GetDraggedInteractionLayerMask(_previousRelevantLayerMask);

            enabled = true;

            _interactor.OnInteractionEnded -= HandleInteractionEnded;
            _interactor.OnInteractionEnded += HandleInteractionEnded;
        }

        private void HandleInteractionEnded(IInteractable interactable)
        {
            if (!ReferenceEquals(interactable, _draggedObject)) return;

            _draggedObject.SetOnDraggedLayer(false);
            _draggedObject.Release((_aimer.HitPosition - _draggedObject.Position) * _dropForce);

            _aimer.HitMask = _previousHitLayerMask;
            _aimer.RelevantMask = _previousRelevantLayerMask;

            enabled = false;
        }

        private bool TryGetDragAimCatcher(out PlayerDragAimCatcher aimCatcher)
        {
            if (_aimer.AimedHit && _aimer.AimedHit.layer == IDraggable.DragAimCatcherLayer)
            {
                if (_aimCatchers.TryGetValue(_aimer.AimedHit, out aimCatcher))
                {
                    return true;
                }

                aimCatcher = _aimer.AimedHit.GetComponent<PlayerDragAimCatcher>();
                _aimCatchers.Add(_aimer.AimedHit, aimCatcher);

                return true;
            }

            aimCatcher = null;
            return false;
        }

        private Vector3 GetDraggedObjectTargetPosition()
        {
            if (TryGetDragAimCatcher(out var aimCatcher))
            {
                return Vector3.Lerp(_playerLookOrigin.position, _aimer.HitPosition, aimCatcher.DragLerpDistance);
            }

            return Vector3.Lerp(_playerLookOrigin.position, _aimer.HitPosition, _defaultLerpDistanceFromPlayer);
        }

        private void Update()
        {
            if (_draggedObject == null)
            {
                enabled = false;
                return;
            }

            _draggedObject.Velocity = Vector3.SmoothDamp(_draggedObject.Velocity, (GetDraggedObjectTargetPosition() - _draggedObject.Position) * _stickyCoefficient, ref _draggedObjectAcceleration, _dampHardness, _maxSpeed);
        }

        private void OnDrawGizmos()
        {
            if (!_aimer) return;
            if (!_playerLookOrigin) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetDraggedObjectTargetPosition(), .1f);
        }
    }
}