using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class RotatorMechanism : MonoBehaviour
    {
        [SerializeField] private ToggleInteractable _interactable;
        [SerializeField] private Transform _objectToRotate;
        [SerializeField] private Vector3 _axis = Vector3.up;
        [SerializeField] private float _speed = 120;
        [SerializeField] private float _acceleration = 80;
        [SerializeField] private float _deceleration = 30;

        private float _currentSpeed;

        private void Update()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _interactable.IsOn ? _speed : 0, (_interactable.IsOn ? _acceleration : _deceleration) * Time.deltaTime);
            _objectToRotate.Rotate(_axis, _currentSpeed * Time.deltaTime);
        }
    }
}