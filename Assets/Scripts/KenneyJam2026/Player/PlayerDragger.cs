using System;
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

        public IDraggable DraggedObject { get; private set; }
        private Vector3 _draggedObjectAcceleration;
        private LayerMask _previousHitLayerMask;
        private LayerMask _previousRelevantLayerMask;

        private readonly Dictionary<GameObject, PlayerDragAimCatcher> _aimCatchers = new();
        public event Action<IDraggable> OnDraggedObjectChanged;

        public bool IsDragging => enabled && DraggedObject != null;
        public Vector3 DraggedObjectDesiredUp { get; set; } = Vector3.up;

        private void Start()
        {
            _interactor.OnInteractionStarted += HandleInteractionStarted;
        }

        private void OnDisable()
        {
            _interactor.OnInteractionEnding -= HandleInteractionEnding;
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

            DraggedObject = draggable;
            DraggedObject.SetOnDraggedLayer(true);
            _draggedObjectAcceleration = Vector3.zero;

            _previousHitLayerMask = _aimer.HitMask;
            _previousRelevantLayerMask = _aimer.RelevantMask;

            _aimer.HitMask = DraggedObject.GetDraggedInteractionLayerMask(_previousHitLayerMask);
            _aimer.RelevantMask = DraggedObject.GetDraggedInteractionLayerMask(_previousRelevantLayerMask);

            enabled = true;

            _interactor.OnInteractionEnding -= HandleInteractionEnding;
            _interactor.OnInteractionEnding += HandleInteractionEnding;
            _interactor.OnInteractionEnded -= HandleInteractionEnded;
            _interactor.OnInteractionEnded += HandleInteractionEnded;

            OnDraggedObjectChanged?.Invoke(DraggedObject);
        }

        private void HandleInteractionEnding(IInteractable interactable)
        {
            if (!ReferenceEquals(interactable, DraggedObject)) return;

            if (_aimer.AimedInteractable != null && _aimer.AimedInteractable.CanInteractWith(DraggedObject))
            {
                _aimer.AimedInteractable.Interact(_aimer.HitPosition, DraggedObject);
            }
        }

        private void HandleInteractionEnded(IInteractable interactable)
        {
            if (!ReferenceEquals(interactable, DraggedObject)) return;

            if (DraggedObject != null)
            {
                DraggedObject.SetOnDraggedLayer(false);
                DraggedObject.Release((_aimer.HitPosition - DraggedObject.Position) * _dropForce);
            }

            _aimer.HitMask = _previousHitLayerMask;
            _aimer.RelevantMask = _previousRelevantLayerMask;

            DraggedObject = null;
            enabled = false;

            OnDraggedObjectChanged?.Invoke(DraggedObject);
        }

        private bool TryGetDragAimCatcher(out PlayerDragAimCatcher aimCatcher)
        {
            if (_aimer.AimedHit && _aimer.AimedHit.layer == IDraggable.DragAimCatcherLayer)
            {
                if (_aimCatchers.TryGetValue(_aimer.AimedHit, out aimCatcher))
                {
                    return aimCatcher;
                }

                aimCatcher = _aimer.AimedHit.GetComponent<PlayerDragAimCatcher>();
                _aimCatchers.Add(_aimer.AimedHit, aimCatcher);

                return aimCatcher;
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
            if (DraggedObject == null)
            {
                enabled = false;
                return;
            }

            DraggedObject.Velocity = Vector3.SmoothDamp(DraggedObject.Velocity, (GetDraggedObjectTargetPosition() - DraggedObject.Position) * _stickyCoefficient, ref _draggedObjectAcceleration, _dampHardness, _maxSpeed);
            DraggedObject.RotateToPosition(_playerLookOrigin.forward, DraggedObjectDesiredUp);
        }
    }
}