using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IDraggable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject[] _interactableLayerObjects;

        public InteractableType InteractableType => _type;

        public Vector3 Position => transform.position;

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact(Vector3 atPosition)
        {
            _rigidbody.useGravity = false;
        }

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

        public void StopInteraction()
        {
            _rigidbody.useGravity = true;
        }
    }
}