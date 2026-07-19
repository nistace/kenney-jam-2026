using System.Collections.Generic;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Spawners
{
    public class DraggableSpawnerManager : MonoBehaviour
    {
        private readonly Dictionary<DraggableObject, DraggableSpawner> _spawnerPerInstance = new();
        private readonly List<DraggableSpawner> _spawnersToTrigger = new();
        private float _nextSpawnTime;

        private void Start()
        {
            _spawnersToTrigger.AddRange(GetComponentsInChildren<DraggableSpawner>());
            FlushSpawns();
        }

        private void FlushSpawns()
        {
            foreach (var spawner in _spawnersToTrigger)
            {
                SpawnFrom(spawner);
            }

            _spawnersToTrigger.Clear();
        }

        private void HandleInstanceDestroyed(DraggableObject destroyedInstance)
        {
            destroyedInstance.OnDestroying -= HandleInstanceDestroyed;
            var spawner = _spawnerPerInstance[destroyedInstance];
            _spawnerPerInstance.Remove(destroyedInstance);

            if (_spawnersToTrigger.Count == 0) _nextSpawnTime = Time.time + Random.Range(.4f, 1f);
            _spawnersToTrigger.Add(spawner);
        }

        private void Update()
        {
            if (_spawnersToTrigger.Count == 0) return;
            if (Time.time < _nextSpawnTime) return;

            SpawnFrom(_spawnersToTrigger[0]);
            _spawnersToTrigger.RemoveAt(0);
            _nextSpawnTime = Time.time + Random.Range(.4f, 1f);
        }

        private void SpawnFrom(DraggableSpawner fromSpawner)
        {
            var instance = fromSpawner.Spawn();
            instance.OnDestroying += HandleInstanceDestroyed;

            _spawnerPerInstance.Add(instance, fromSpawner);
        }
    }
}