using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public interface IDraggable : IInteractable
    {
        public static int InteractableLayer => LayerMask.NameToLayer("Interactable");
        public static int DraggedInteractableLayer => LayerMask.NameToLayer("DraggedInteractable");
        public static int DragAimCatcherLayer => LayerMask.NameToLayer("DragAimCatcher");
        public static int DragAimCatcherLayerMask => 1 << DragAimCatcherLayer;

        InteractableType DraggingInteractionType { get; }
        Vector3 Position { get; }
        Vector3 Velocity { get; set; }

        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        void Release(Vector3 intendedForce);
        void SetOnDraggedLayer(bool value);
        LayerMask GetDraggedInteractionLayerMask(LayerMask currentLayerMask);
        void RotateToPosition(Vector3 forward, Vector3 up);
    }
}