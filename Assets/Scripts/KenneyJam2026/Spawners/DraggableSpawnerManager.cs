using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Spawners
{
    public class DraggableSpawnerManager : MonoBehaviour
    {
        private readonly Dictionary<DraggableObject, DraggableSpawner> _spawnerPerInstance = new();

        private void Start()
        {
            foreach (var spawner in GetComponentsInChildren<DraggableSpawner>())
            {
                var instance = spawner.Spawn();
                _spawnerPerInstance.Add(instance, spawner);
                instance.OnDestroying += HandleInstanceDestroyed;
            }
        }

        private void HandleInstanceDestroyed(DraggableObject destroyedInstance)
        {
            destroyedInstance.OnDestroying -= HandleInstanceDestroyed;
            var spawner = _spawnerPerInstance[destroyedInstance];
            _spawnerPerInstance.Remove(destroyedInstance);

            if (!destroyCancellationToken.IsCancellationRequested)
            {
                _ = DelaySpawn(Random.Range(.5f, 1), spawner, destroyCancellationToken);
            }
        }

        private async UniTask DelaySpawn(float _delay, DraggableSpawner spawner, CancellationToken cancellationToken)
        {
            await UniTask.Delay((int)(_delay * 1000), cancellationToken: cancellationToken);

            SpawnFrom(spawner);
        }

        private void SpawnFrom(DraggableSpawner fromSpawner)
        {
            var instance = fromSpawner.Spawn();
            _spawnerPerInstance.Add(instance, fromSpawner);
            instance.OnDestroying += HandleInstanceDestroyed;
        }
    }
}