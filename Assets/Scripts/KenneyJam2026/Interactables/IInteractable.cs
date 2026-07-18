using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public interface IInteractable
    {
        InteractableType InteractableType { get; }

        public bool CanInteractWith(IDraggable heldObject);
        void Interact(Vector3 atPosition);
        void StopInteraction();
    }
}