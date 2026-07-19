using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2026.Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private InputActionReference _lookActionReference;
        [SerializeField] private Transform _lookRotationTransform;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _pitchMin = -80;
        [SerializeField] private float _pitchMax = 80;
        [SerializeField] private float _yawOnStart = 90;
        [SerializeField] private float _pitchOnStart;

        private float _yaw = 90;
        private float _pitch;

        private void OnEnable()
        {
            _yaw = _yawOnStart;
            _pitch = _pitchOnStart;
            _lookActionReference.action.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            _lookActionReference.action.Disable();
        }

        private void Update()
        {
            var inputValue = _lookActionReference.action.ReadValue<Vector2>();

            _pitch = Mathf.MoveTowards(_pitch, inputValue.y > 0 ? _pitchMin : _pitchMax, Mathf.Abs(inputValue.y) * _speed);
            _yaw += inputValue.x * _speed;

            _lookRotationTransform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
        }
    }
}