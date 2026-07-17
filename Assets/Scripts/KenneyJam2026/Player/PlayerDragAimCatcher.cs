using UnityEngine;

namespace KenneyJam2026.Player
{
    public class PlayerDragAimCatcher : MonoBehaviour
    {
        [SerializeField] private float _dropForce;
        [SerializeField] private float _dragLerpDistance = 1;

        public float DropForce => _dropForce;
        public float DragLerpDistance => _dragLerpDistance;
    }
}