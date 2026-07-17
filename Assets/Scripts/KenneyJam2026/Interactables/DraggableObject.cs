using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _type;

        public InteractableType Type => _type;

        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}