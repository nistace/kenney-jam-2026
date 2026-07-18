using System;
using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class WindowMechanism : MonoBehaviour
    {
        [SerializeField] private SlidingObject _slidingObject;
        [SerializeField] private float _openThreshold = 1;

        public event Action<bool> OnOpenedChanged;
        public bool IsOpen { get; private set; }

        private void Start()
        {
            IsOpen = _slidingObject.CurrentOffset > _openThreshold;
            _slidingObject.OnMoved += HandleWindowMoved;
        }

        private void HandleWindowMoved(float currentOffset)
        {
            if (currentOffset > _openThreshold == IsOpen) return;

            IsOpen = !IsOpen;
            OnOpenedChanged?.Invoke(IsOpen);
        }
    }
}