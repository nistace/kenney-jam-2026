using KenneyJam2026.Interactables;
using KenneyJam2026.Player;
using UnityEngine;
using UnityEngine.UI;

namespace KenneyJam2026.PlayerUI
{
    public class InteractionDisplay : MonoBehaviour
    {
        [SerializeField] private Image _displayImage;
        [SerializeField] private PlayerAimDetector _aimDetector;
        [SerializeField] private PlayerDragger _dragger;
        [SerializeField] private InteractableType _draggingInteractionType;

        private void Update()
        {
            if (_dragger && _dragger.IsDragging)
            {
                _displayImage.overrideSprite = _draggingInteractionType.GetTimedCursorSprite();
            }
            else if (_aimDetector && _aimDetector.AimedInteractable is { Type: { HasCursorSprites: true } })
            {
                _displayImage.overrideSprite = _aimDetector.AimedInteractable.Type.GetTimedCursorSprite();
            }
            else
            {
                _displayImage.overrideSprite = null;
            }

            _displayImage.enabled = _displayImage.overrideSprite;
        }
    }
}