using System;
using System.Collections.Generic;
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
        [SerializeField] private int _highestNpcLevelCompleted;
        [SerializeField] private float _firstAutoSpawn = 180;
        [SerializeField] private float _timeBetweenSpawns = 30;

        private Npc _lineHeadNpc;

        public int HighestNpcLevelCompleted
        {
            get => _highestNpcLevelCompleted;
            private set => _highestNpcLevelCompleted = value;
        }

        private int SpawnableLevel => HighestNpcLevelCompleted + 1;
        private readonly Queue<NpcType> _nextNpcQueue = new();

        private float _nextAutoSpawnTime;

        public event Action<int> OnHighestNpcLevelCompletedIncreased;

        private void Start()
        {
            _nextAutoSpawnTime = _firstAutoSpawn;
            _line.OnHeadOwnerChanged += HandleLineHeadOwnerChanged;
            _windowMechanism.OnOpenedChanged += HandleWindowsOpenStateChanged;

            RefreshNextNpcQueue();
        }

        private void RefreshNextNpcQueue()
        {
            _nextNpcQueue.Clear();

            foreach (var type in _types)
            {
                if (type.Level <= SpawnableLevel)
                {
                    _nextNpcQueue.Enqueue(type);
                }
            }
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
            _lineHeadNpc.OnDeliveryFulfilled += HandleDeliveryFulfilled;
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
            _lineHeadNpc.OnDeliveryFulfilled -= HandleDeliveryFulfilled;
        }

        private void HandleLineHeadOwnerChanged()
        {
            UnsubscribeNpcEvents();

            _lineHeadNpc = _line.Head.Owner;
            if (_lineHeadNpc != null)
            {
                _lineHeadNpc.SetWindowOpen(_windowMechanism.IsOpen);
            }

            SubscribeNpcEvents();
        }

        [ContextMenu("TrySpawnNewNpc")]
        private void TrySpawnNewNpc()
        {
            if (!_line.HasEmptySlots) return;

            if (_nextNpcQueue.Count == 0)
            {
                RefreshNextNpcQueue();
            }

            var spawnedType = _nextNpcQueue.Dequeue();

            var spawn = _spawns[Random.Range(0, _spawns.Length)];
            var newNpc = spawn.Spawn(_emptyNpcPrefab, spawnedType);
            newNpc.SetWindowOpen(_windowMechanism.IsOpen);
            _line.AssignNextPositionInLine(newNpc);

            _nextAutoSpawnTime = Time.time + _timeBetweenSpawns;
        }

        private void HandleDeliveryFulfilled(Npc npc)
        {
            if (npc.Type.Level <= HighestNpcLevelCompleted) return;

            HighestNpcLevelCompleted = npc.Type.Level;
            RefreshNextNpcQueue();
            OnHighestNpcLevelCompletedIncreased?.Invoke(HighestNpcLevelCompleted);
        }

        private void HandleWindowsOpenStateChanged(bool windowOpen)
        {
            if (!_line.HasOccupiedSLots)
            {
                TrySpawnNewNpc();
            }

            if (_lineHeadNpc)
            {
                _lineHeadNpc.SetWindowOpen(windowOpen);
            }
        }

        private void HandleNpcMessage(NpcMessage npcMessage) => _bubblesCanvas.ShowMessage(npcMessage);
        private void HandleNpcMessageCancelled() => _bubblesCanvas.ClearMessage();

        private void Update()
        {
            if (Time.time < _nextAutoSpawnTime) return;

            TrySpawnNewNpc();
            _nextAutoSpawnTime = Time.time + _timeBetweenSpawns;
        }
    }
}