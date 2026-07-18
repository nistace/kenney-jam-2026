using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IDraggable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private InteractableType _draggingInteractionType;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject[] _interactableLayerObjects;

        public InteractableType InteractableType => _type;
        public InteractableType DraggingInteractionType => _draggingInteractionType;
        public Vector3 Position => transform.position;
        public GameObject GameObject { get; }

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public bool CanInteractWith(IDraggable heldObject) => heldObject == null;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            if (heldObject != null) return;

            _rigidbody.useGravity = false;
        }

        public void StopInteraction() => _rigidbody.useGravity = true;

        public void SetOnDraggedLayer(bool value)
        {
            var newLayer = value ? IDraggable.DraggedInteractableLayer : IDraggable.InteractableLayer;

            foreach (var layerObject in _interactableLayerObjects)
            {
                layerObject.layer = newLayer;
            }
        }

        public LayerMask GetDraggedInteractionLayerMask(LayerMask currentLayerMask) => currentLayerMask | IDraggable.DragAimCatcherLayerMask;


        public void Release(Vector3 intendedForce)
        {
            _rigidbody.AddForce(intendedForce, ForceMode.Impulse);
        }
    }
}