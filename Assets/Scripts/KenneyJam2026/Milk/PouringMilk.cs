using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class PouringMilk : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private readonly Dictionary<GameObject, MilkPouringReceiver> _receivers = new();

        private void OnParticleCollision(GameObject other)
        {
            if (!_receivers.TryGetValue(other, out var receiver))
            {
                Debug.Log("Particle collided with " + other.gameObject);

                receiver = other.GetComponent<MilkPouringReceiver>();
                _receivers.Add(other, receiver);
            }

            if (!receiver)
            {
                return;
            }

            Debug.Log("Pouring hit receiver!");

            receiver.Feed();
        }
    }
}