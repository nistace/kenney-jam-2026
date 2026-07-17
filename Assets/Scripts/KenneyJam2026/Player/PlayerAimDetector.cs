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

        private GameObject _aimedHit;
        public IInteractable AimedInteractable { get; private set; }

        public event Action<IInteractable> OnAimedInteractableChanged;

        private void Update()
        {
            if (Physics.Raycast(new Ray(_aimer.position, _aimer.forward), out var hit, _maxDistance, _hitMask) && (_relevantMask & (1 << hit.collider.gameObject.layer)) > 0)
            {
                if (hit.collider.gameObject == _aimedHit)
                {
                    return;
                }

                _aimedHit = hit.collider.gameObject;
                var previousHitInteractable = AimedInteractable;
                AimedInteractable = _aimedHit.GetComponentInParent<IInteractable>();

                if (AimedInteractable != previousHitInteractable)
                {
                    OnAimedInteractableChanged?.Invoke(AimedInteractable);
                }
            }
            else if (_aimedHit != null)
            {
                _aimedHit = null;
                AimedInteractable = null;
                OnAimedInteractableChanged?.Invoke(null);
            }
        }
    }
}