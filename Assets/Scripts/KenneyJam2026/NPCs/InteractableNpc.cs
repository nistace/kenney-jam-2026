using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class InteractableNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _interactToAskQuantity;
        [SerializeField] private InteractableType _interactToDeliver;
        [SerializeField] private Npc _npc;

        public InteractableType InteractableType => _npc.CurrentState switch
        {
            Npc.ECurrentState.AskMilk => _interactToAskQuantity,
            Npc.ECurrentState.AskQuantity or Npc.ECurrentState.WaitMilk => _interactToDeliver,
            _ => default
        };

        public bool CanInteractWith(IDraggable heldObject) => _npc.CurrentState switch
        {
            Npc.ECurrentState.AskMilk => heldObject == null,
            Npc.ECurrentState.AskQuantity or Npc.ECurrentState.WaitMilk => heldObject != null,
            _ => false
        };

        public void Interact(Vector3 atPosition)
        {
            if (_npc.CurrentState == Npc.ECurrentState.AskMilk)
            {
                _npc.AskQuantity();
            }
        }

        public void StopInteraction()
        {
        }
    }
}