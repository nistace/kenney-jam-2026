using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class NpcPositionInLine : MonoBehaviour
    {
        [SerializeField] private Npc _owner;

        public bool HeadPositionInLine { get; set; }

        public Npc Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                if (!_owner) return;

                _owner.SetTargetPositionInLine(transform.position, HeadPositionInLine);
                _owner.TargetForward = transform.forward;
            }
        }
    }
}