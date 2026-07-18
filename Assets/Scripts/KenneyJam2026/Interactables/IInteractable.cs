using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public interface IInteractable
    {
        InteractableType InteractableType { get; }

        void Interact(Vector3 atPosition);
        void StopInteraction();
    }
}