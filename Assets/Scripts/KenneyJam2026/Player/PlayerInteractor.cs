using System;
using KenneyJam2026.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2026.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerAimDetector _detector;
        [SerializeField] private InputActionReference _interactActionReference;

        private IInteractable _currentInteractable;
        private bool IsInteracting => _currentInteractable != null;

        public event Action<IInteractable> OnInteractionStarted;
        public event Action<IInteractable> OnInteractionEnded;

        private void OnEnable()
        {
            _interactActionReference.action.Enable();
            _interactActionReference.action.performed += HandleInteractPerformed;
            _interactActionReference.action.canceled += HandleInteractCancelled;
        }

        private void OnDisable()
        {
            _interactActionReference.action.Disable();
            _interactActionReference.action.performed -= HandleInteractPerformed;
            _interactActionReference.action.canceled -= HandleInteractCancelled;
        }

        private void HandleInteractCancelled(InputAction.CallbackContext obj)
        {
            if (_currentInteractable == null)
            {
                return;
            }

            var interactable = _currentInteractable;
            _currentInteractable.StopInteraction();
            _currentInteractable = null;

            OnInteractionEnded?.Invoke(interactable);
        }

        private void HandleInteractPerformed(InputAction.CallbackContext obj)
        {
            if (_detector.AimedInteractable == null)
            {
                return;
            }

            _currentInteractable = _detector.AimedInteractable;
            _currentInteractable.Interact();
            OnInteractionStarted?.Invoke(_currentInteractable);
        }
    }
}