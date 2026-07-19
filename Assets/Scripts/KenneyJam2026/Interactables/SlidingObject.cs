using System;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class SlidingObject : MonoBehaviour, IDraggable
    {
        [SerializeField] private InteractableType _interactableType;
        [SerializeField] private InteractableType _draggingInteractionType;
        [SerializeField] private float _minValue = -1;
        [SerializeField] private float _maxValue = 1;
        [SerializeField] private float _speed = 1;
        [SerializeField] private GameObject[] _objectsToActivateWhileDragging;

        public InteractableType InteractableType => _interactableType;
        public string ID { get; }
        public InteractableType DraggingInteractionType => _draggingInteractionType;
        private Vector3 _offsetOnInteractionStarted;

        public Vector3 Position => transform.position + _offsetOnInteractionStarted;
        public Vector3 Velocity { get; set; }
        public float CurrentOffset => transform.localPosition.y;

        public event Action<float> OnMoved;

        private void OnEnable() => RefreshObjectToActiveWhileDragging();
        private void OnDisable() => RefreshObjectToActiveWhileDragging();

        private void RefreshObjectToActiveWhileDragging()
        {
            foreach (var objectToActivateWhileDragging in _objectsToActivateWhileDragging)
            {
                if (objectToActivateWhileDragging != null)
                {
                    objectToActivateWhileDragging.SetActive(true);
                }
            }
        }

        public void Interact() { }

        public bool CanInteractWith(IDraggable heldObject) => heldObject == null;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            if (heldObject != null) return;

            _offsetOnInteractionStarted = new Vector3(0, atPosition.y - transform.position.y, 0);
            Velocity = Vector3.zero;
            enabled = true;
        }

        public void StopInteraction()
        {
            enabled = false;
            foreach (var objectToActivateWhileDragging in _objectsToActivateWhileDragging)
            {
                objectToActivateWhileDragging.SetActive(false);
            }
        }

        public void SetOnDraggedLayer(bool dragged)
        {
            gameObject.layer = dragged ? IDraggable.DraggedInteractableLayer : IDraggable.InteractableLayer;
        }

        public LayerMask GetDraggedInteractionLayerMask(LayerMask currentLayerMask) => IDraggable.DragAimCatcherLayerMask;
        public void RotateToPosition(Vector3 forward, Vector3 up) { }
        public void Release(Vector3 intendedForce) { }

        private void FixedUpdate()
        {
            var newY = Mathf.Clamp(transform.localPosition.y + Velocity.y * (_speed * Time.deltaTime), _minValue, _maxValue);

            if (Mathf.Approximately(transform.localPosition.y, newY))
            {
                return;
            }

            transform.localPosition = new Vector3(0, newY, 0);

            OnMoved?.Invoke(newY);
        }
    }
}