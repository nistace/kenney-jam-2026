using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class Trashcan : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _interactableType;

        public InteractableType InteractableType => _interactableType;

        public bool CanInteractWith(IDraggable heldObject) => heldObject != null;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            if (heldObject == null) return;

            Destroy(heldObject.gameObject);
        }

        public void StopInteraction() { }
    }
}