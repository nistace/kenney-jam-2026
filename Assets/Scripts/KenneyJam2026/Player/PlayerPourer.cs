using KenneyJam2026.Interactables;
using KenneyJam2026.Milk;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2026.Player
{
    public class PlayerPourer : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pourActionReference;
        [SerializeField] private PlayerDragger _dragger;

        private MilkPourer _pourer;
        private bool _pouring;

        private void Start()
        {
            _pourActionReference.action.Enable();
            _pourActionReference.action.performed += HandlePourPerformed;
            _pourActionReference.action.canceled += HandlePourCancelled;
            _dragger.OnDraggedObjectChanged += HandleDraggedObjectChanged;
        }

        private void OnDestroy()
        {
            _pourActionReference.action.Disable();
            _pourActionReference.action.performed -= HandlePourPerformed;
            _pourActionReference.action.canceled -= HandlePourCancelled;
            _dragger.OnDraggedObjectChanged -= HandleDraggedObjectChanged;
        }

        private void HandlePourCancelled(InputAction.CallbackContext obj)
        {
            _dragger.DraggedObjectDesiredUp = Vector3.up;
            _pouring = false;
            enabled = false;
        }

        private void HandlePourPerformed(InputAction.CallbackContext obj)
        {
            _pouring = true;
            enabled = true;
        }

        private void HandleDraggedObjectChanged(IDraggable currentDraggedObject)
        {
            _pourer = currentDraggedObject?.gameObject.GetComponent<MilkPourer>();
            _pouring = false;
        }

        private void Update()
        {
            if (!_pourer || !_pouring)
            {
                enabled = false;
                return;
            }

            _dragger.DraggedObjectDesiredUp = _pourer.GetRequiredUpToPourAtLowerCharge();
        }
    }
}