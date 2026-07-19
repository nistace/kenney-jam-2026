using System;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _interactableType;
        [SerializeField] private DraggableObject _keyPrefab;
        public InteractableType InteractableType => _interactableType;

        public event Action OnExitTriggered;

        public bool CanInteractWith(IDraggable heldObject) => heldObject is DraggableObject draggedObject && _keyPrefab.Identifier == draggedObject.Identifier;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            if (!CanInteractWith(heldObject) || heldObject is not DraggableObject draggableObject) return;

            Destroy(draggableObject.gameObject);
            OnExitTriggered?.Invoke();
        }

        public void StopInteraction() { }
    }
}