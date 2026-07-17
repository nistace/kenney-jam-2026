using System;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Player
{
    public class PlayerAimDetector : MonoBehaviour
    {
        [SerializeField] private Transform _aimer;
        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private LayerMask _relevantMask;
        [SerializeField] private float _maxDistance = 20;

        public GameObject AimedHit { get; private set; }
        public Vector3 HitPosition { get; private set; }
        public IInteractable AimedInteractable { get; private set; }

        public LayerMask HitMask
        {
            get => _hitMask;
            set => _hitMask = value;
        }

        public LayerMask RelevantMask
        {
            get => _relevantMask;
            set => _relevantMask = value;
        }

        public event Action<IInteractable> OnAimedInteractableChanged;

        private void Update()
        {
            if (Physics.Raycast(new Ray(_aimer.position, _aimer.forward), out var hit, _maxDistance, _hitMask))
            {
                HitPosition = hit.point;

                if ((_relevantMask & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    if (hit.collider.gameObject == AimedHit)
                    {
                        return;
                    }

                    AimedHit = hit.collider.gameObject;
                    var previousHitInteractable = AimedInteractable;
                    AimedInteractable = AimedHit.GetComponentInParent<IInteractable>();

                    if (AimedInteractable != previousHitInteractable)
                    {
                        OnAimedInteractableChanged?.Invoke(AimedInteractable);
                    }

                    return;
                }
            }

            if (AimedHit == null)
            {
                return;
            }

            AimedHit = null;
            AimedInteractable = null;
            OnAimedInteractableChanged?.Invoke(null);
        }
    }
}