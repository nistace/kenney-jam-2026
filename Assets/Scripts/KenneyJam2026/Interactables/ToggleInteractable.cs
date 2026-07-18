using System;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class ToggleInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private bool _isOn;

        public bool IsOn => _isOn;
        public InteractableType InteractableType => _type;

        public event Action<bool> OnIsOnChanged;

        public void Interact(Vector3 atPosition)
        {
            _isOn = !_isOn;

            OnIsOnChanged?.Invoke(_isOn);
        }

        public void StopInteraction() { }
    }
}