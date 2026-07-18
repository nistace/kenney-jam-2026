using KenneyJam2026.Mechanisms;
using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KenneyJam2026.NPCs
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField] private NpcSpawn[] _spawns;
        [SerializeField] private NpcLine _line;
        [SerializeField] private NpcType[] _types;
        [SerializeField] private Npc _emptyNpcPrefab;
        [SerializeField] private NpcBubblesCanvas _bubblesCanvas;
        [SerializeField] private WindowMechanism _windowMechanism;

        private Npc _lineHeadNpc;

        private void Start()
        {
            _line.OnHeadOwnerChanged += HandleLineHeadOwnerChanged;
            _windowMechanism.OnOpenedChanged += HandleWindowsOpenStateChanged;

            TrySpawnNewNpc();
        }

        private void OnDestroy()
        {
            _line.OnHeadOwnerChanged -= HandleLineHeadOwnerChanged;
            _windowMechanism.OnOpenedChanged -= HandleWindowsOpenStateChanged;

            UnsubscribeNpcEvents();
        }

        private void SubscribeNpcEvents()
        {
            if (!_lineHeadNpc) return;

            _lineHeadNpc.OnMessageArticulated += HandleNpcMessage;
            _lineHeadNpc.OnMessageCancelled += HandleNpcMessageCancelled;
            _lineHeadNpc.OnWantToLeave += HandleNpcWantToLeave;
        }

        private void HandleNpcWantToLeave(Npc npc)
        {
            npc.SetTargetPositionToLeave(_spawns[Random.Range(0, _spawns.Length)].transform.position);
            _line.Unassign(npc);
        }

        private void UnsubscribeNpcEvents()
        {
            if (!_lineHeadNpc) return;

            _lineHeadNpc.OnMessageArticulated -= HandleNpcMessage;
            _lineHeadNpc.OnMessageCancelled -= HandleNpcMessageCancelled;
            _lineHeadNpc.OnWantToLeave -= HandleNpcWantToLeave;
        }

        private void HandleLineHeadOwnerChanged()
        {
            UnsubscribeNpcEvents();

            _lineHeadNpc = _line.Head.Owner;

            SubscribeNpcEvents();
        }

        [ContextMenu("TrySpawnNewNpc")]
        private void TrySpawnNewNpc()
        {
            if (!_line.HasEmptySlots) return;

            var spawn = _spawns[Random.Range(0, _spawns.Length)];
            var type = _types[Random.Range(0, _types.Length)];
            var newNpc = spawn.Spawn(_emptyNpcPrefab, type);
            newNpc.SetWindowOpen(_windowMechanism.IsOpen);
            _line.AssignNextPositionInLine(newNpc);
        }

        private void HandleWindowsOpenStateChanged(bool windowOpen)
        {
            if (_lineHeadNpc)
            {
                _lineHeadNpc.SetWindowOpen(windowOpen);
            }
        }

        private void HandleNpcMessage(NpcMessage npcMessage) => _bubblesCanvas.ShowMessage(npcMessage);
        private void HandleNpcMessageCancelled() => _bubblesCanvas.ClearMessage();
    }
}