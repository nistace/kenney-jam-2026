using System;
using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class NpcLine : MonoBehaviour
    {
        [SerializeField] private NpcPositionInLine[] _positions;

        public NpcPositionInLine Head => _positions[0];
        public bool HasEmptySlots => _positions[^1].Owner == null;

        public event Action OnHeadOwnerChanged;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var position in _positions)
            {
                Gizmos.DrawSphere(position.transform.position, 0.1f);
            }
        }

        private void Awake()
        {
            _positions[0].HeadPositionInLine = true;
        }

        public NpcPositionInLine AssignNextPositionInLine(Npc npc)
        {
            if (!HasEmptySlots)
            {
                return null;
            }

            var firstEmptySlot = 0;
            while (_positions[firstEmptySlot].Owner != null)
            {
                firstEmptySlot++;
            }

            _positions[firstEmptySlot].Owner = npc;

            if (firstEmptySlot == 0)
            {
                OnHeadOwnerChanged?.Invoke();
            }

            return _positions[firstEmptySlot];
        }

        public void Unassign(Npc npc)
        {
            var unassignFromIndex = 0;

            while (unassignFromIndex < _positions.Length && _positions[unassignFromIndex].Owner != npc)
            {
                unassignFromIndex++;
            }

            if (unassignFromIndex == _positions.Length)
            {
                return;
            }

            _positions[unassignFromIndex].Owner = null;

            for (var i = unassignFromIndex; i < _positions.Length; i++)
            {
                _positions[i].Owner = i >= _positions.Length - 1 ? null : _positions[i + 1].Owner;
            }

            if (unassignFromIndex == 0)
            {
                OnHeadOwnerChanged?.Invoke();
            }
        }
    }
}