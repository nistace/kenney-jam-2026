using System;
using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KenneyJam2026.NPCs
{
    [CreateAssetMenu]
    public class NpcType : ScriptableObject
    {
        [SerializeField] private GameObject _modelPrefab;
        [SerializeField] private NpcExpectedQuantity[] _possibleExpectedQuantities;
        [SerializeField] private int _level = 1;
        [SerializeField] private float _movementSpeed = 2;
        [SerializeField] private float _scale = 2.5f;

        [SerializeField] private float _maxDurationInLine = 240;
        [SerializeField] private float _delayBeforeKnocking = 5;
        [SerializeField] private NpcMessage _messageWhileKnocking;
        [SerializeField] private float _maxDurationKnocking = 10;
        [SerializeField] private NpcMessage _askMilkMessage;
        [SerializeField] private NpcMessage _refuseDeliveryMessage;
        [SerializeField] private NpcMessage _acceptDeliveryMessage;
        [SerializeField] private float _timeAfterHandlingDelivery = 2;
        [SerializeField] private NpcMessage _messageWhileCancelling;
        [SerializeField] private float _cancelRequestDuration = 1;

        [SerializeField] private AudioData _audio;

        public GameObject ModelPrefab => _modelPrefab;
        public int Level => _level;
        public NpcMessage AskMilkMessage => _askMilkMessage;
        public float MovementSpeed => _movementSpeed;
        public float Scale => _scale;
        public float CancelRequestDuration => _cancelRequestDuration;
        public float DelayBeforeKnocking => _delayBeforeKnocking;
        public float MaxDurationKnocking => _maxDurationKnocking;
        public NpcMessage MessageWhileKnocking => _messageWhileKnocking;
        public float MaxDurationInLine => _maxDurationInLine;
        public NpcMessage MessageWhileCancelling => _messageWhileCancelling;
        public NpcMessage RefuseDeliveryMessage => _refuseDeliveryMessage;
        public NpcMessage AcceptDeliveryMessage => _acceptDeliveryMessage;
        public float TimeAfterHandlingDelivery => _timeAfterHandlingDelivery;

        public AudioData Audio => _audio;

        public NpcExpectedQuantity GetRandomExpectedQuantity() => _possibleExpectedQuantities[Random.Range(0, _possibleExpectedQuantities.Length)];

        [Serializable]
        public class AudioData
        {
            [SerializeField] private AudioClip _knock;
            [SerializeField] private AudioClip _askMilk;
            [SerializeField] private AudioClip _askQuantity;
            [SerializeField] private AudioClip _refuseDelivery;
            [SerializeField] private AudioClip _acceptDelivery;
            [SerializeField] private AudioClip _cancel;

            public AudioClip Knock => _knock;

            public AudioClip AskMilk => _askMilk;

            public AudioClip AskQuantity => _askQuantity;

            public AudioClip RefuseDelivery => _refuseDelivery;

            public AudioClip AcceptDelivery => _acceptDelivery;

            public AudioClip Cancel => _cancel;
        }
    }
}