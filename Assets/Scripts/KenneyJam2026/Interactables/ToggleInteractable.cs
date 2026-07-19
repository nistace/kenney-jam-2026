using System;
using KenneyJam2026.Audio;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class ToggleInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private bool _isOn;
        [SerializeField] private AudioSfx _audioSfx;

        public bool IsOn => _isOn;
        public InteractableType InteractableType => _type;

        public event Action<bool> OnIsOnChanged;

        public bool CanInteractWith(IDraggable heldObject) => heldObject == null;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            _audioSfx.Play();

            if (heldObject != null) return;

            _isOn = !_isOn;
            OnIsOnChanged?.Invoke(_isOn);
        }

        public void StopInteraction() { }
    }
}