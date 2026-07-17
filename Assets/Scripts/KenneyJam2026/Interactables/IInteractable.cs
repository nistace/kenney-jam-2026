namespace KenneyJam2026.Interactables
{
    public interface IInteractable
    {
        InteractableType Type { get; }

        void Interact();
    }
}