using System;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private Rigidbody _rigidbody;

        public InteractableType Type => _type;

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
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