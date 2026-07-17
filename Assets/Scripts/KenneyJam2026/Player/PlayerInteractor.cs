using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2026.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerAimDetector _detector;
        [SerializeField] private InputActionReference _interactActionReference;

        private bool IsInteracting { get; set; }

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

        private void HandleInteractCancelled(InputAction.CallbackContext obj) { }

        private void HandleInteractPerformed(InputAction.CallbackContext obj)
        {
            _detector.AimedInteractable?.Interact();
        }
    }
}