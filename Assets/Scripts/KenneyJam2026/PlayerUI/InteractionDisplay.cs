using System;
using KenneyJam2026.Player;
using UnityEngine;
using UnityEngine.UI;

namespace KenneyJam2026.PlayerUI
{
    public class InteractionDisplay : MonoBehaviour
    {
        [SerializeField] private Image _displayImage;
        [SerializeField] private PlayerAimDetector _aimDetector;

        private void Update()
        {
            _displayImage.enabled = _aimDetector && _aimDetector.AimedInteractable is { Type: { HasCursorSprites: true } };

            if (!_displayImage.enabled)
            {
                return;
            }

            _displayImage.sprite = _aimDetector.AimedInteractable.Type.GetTimedCursorSprite();
        }
    }
}