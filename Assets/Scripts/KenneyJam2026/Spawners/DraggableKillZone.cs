using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Spawners
{
    public class DraggableKillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var draggable = other.gameObject.GetComponentInParent<DraggableObject>();

            if (draggable != null)
            {
                Destroy(draggable.gameObject);
            }
        }
    }
}