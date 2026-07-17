using System;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class ToggleInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private bool _isOn;

        public bool IsOn => _isOn;
        public InteractableType Type => _type;

        public event Action<bool> OnIsOnChanged;

        public void Interact()
        {
            _isOn = !_isOn;

            OnIsOnChanged?.Invoke(_isOn);
        }

        public void StopInteraction() { }
    }
}