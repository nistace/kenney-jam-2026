using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;

namespace KenneyJam2026.NPCs
{
    [CreateAssetMenu]
    public class NpcType : ScriptableObject
    {
        [SerializeField] private GameObject _modelPrefab;
        [SerializeField] private NpcExpectedQuantity[] _possibleExpectedQuantities;
        [SerializeField] private float _level = 1;
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

        public GameObject ModelPrefab => _modelPrefab;
        public float Level => _level;
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

        public NpcExpectedQuantity GetRandomExpectedQuantity() => _possibleExpectedQuantities[Random.Range(0, _possibleExpectedQuantities.Length)];
    }
}