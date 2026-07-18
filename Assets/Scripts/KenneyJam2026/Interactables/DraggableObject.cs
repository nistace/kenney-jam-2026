using System;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IInteractable
    {
        private static int InteractableLayer => LayerMask.NameToLayer("Interactable");
        private static int DraggedInteractableLayer => LayerMask.NameToLayer("DraggedInteractable");

        [SerializeField] private InteractableType _type;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject[] _interactableLayerObjects;

        public InteractableType Type => _type;

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        public bool OnDraggedLayer
        {
            set
            {
                var newLayer = value ? DraggedInteractableLayer : InteractableLayer;

                foreach (var layerObject in _interactableLayerObjects)
                {
                    layerObject.layer = newLayer;
                }
            }
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact()
        {
            _rigidbody.useGravity = false;
        }

        public void StopInteraction()
        {
            _rigidbody.useGravity = true;
        }

        public void AddForce(Vector3 force) => _rigidbody.AddForce(force, ForceMode.Impulse);
    }
}