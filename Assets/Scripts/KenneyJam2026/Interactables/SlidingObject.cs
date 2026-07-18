using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class SlidingObject : MonoBehaviour, IDraggable
    {
        [SerializeField] private InteractableType _interactableType;
        [SerializeField] private float _minValue = -1;
        [SerializeField] private float _maxValue = 1;
        [SerializeField] private float _speed = 1;

        public InteractableType InteractableType => _interactableType;
        private Vector3 _offsetOnInteractionStarted;

        public Vector3 Position => transform.position + _offsetOnInteractionStarted;
        public Vector3 Velocity { get; set; }

        public void Interact() { }

        public void Interact(Vector3 atPosition)
        {
            _offsetOnInteractionStarted = new Vector3(0, atPosition.y - transform.position.y, 0);
            Velocity = Vector3.zero;
            enabled = true;
        }

        public void StopInteraction()
        {
            enabled = false;
        }

        public void SetOnDraggedLayer(bool dragged)
        {
            gameObject.layer = dragged ? IDraggable.DraggedInteractableLayer : IDraggable.InteractableLayer;
        }

        public LayerMask GetDraggedInteractionLayerMask(LayerMask currentLayerMask) => IDraggable.DragAimCatcherLayerMask;

        public void Release(Vector3 intendedForce) { }

        private void FixedUpdate()
        {
            transform.localPosition = new Vector3(0, Mathf.Clamp(transform.localPosition.y + Velocity.y * (_speed * Time.deltaTime), _minValue, _maxValue));
        }
    }
}