using KenneyJam2026.Interactables;
using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class Radio : MonoBehaviour
    {
        [SerializeField] private ToggleInteractable _interactable;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private NpcBubble _bubble;

        private void OnEnable()
        {
            _interactable.OnIsOnChanged += HandleInteractableIsOnChanged;

            Refresh();
        }

        private void OnDisable()
        {
            _interactable.OnIsOnChanged -= HandleInteractableIsOnChanged;
        }

        private void HandleInteractableIsOnChanged(bool isOn) => Refresh();

        private void Refresh()
        {
            _audioSource.mute = !_interactable.IsOn;
            if (_interactable.IsOn)
            {
                _bubble.Appear(null);
            }
            else
            {
                _bubble.Disappear();
            }

            if (!_audioSource.mute && !_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
    }
}