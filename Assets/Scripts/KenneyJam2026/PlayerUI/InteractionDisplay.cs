using KenneyJam2026.Interactables;
using KenneyJam2026.Player;
using UnityEngine;
using UnityEngine.UI;

namespace KenneyJam2026.PlayerUI
{
    public class InteractionDisplay : MonoBehaviour
    {
        [SerializeField] private Image _displayImage;
        [SerializeField] private Image _secondaryDisplayImage;
        [SerializeField] private PlayerAimDetector _aimDetector;
        [SerializeField] private PlayerDragger _dragger;

        private void Update()
        {
            InteractableType interactionType = default;

            if (_dragger && _dragger.IsDragging)
            {
                interactionType = _dragger.DraggedObject.DraggingInteractionType;
            }
            else if (_aimDetector && _aimDetector.AimedInteractable != null)
            {
                interactionType = _aimDetector.AimedInteractable.InteractableType;
            }

            _displayImage.overrideSprite = interactionType?.GetTimedCursorSprite();
            _displayImage.enabled = _displayImage.overrideSprite;

            _secondaryDisplayImage.overrideSprite = interactionType?.GetTimedSecondaryCursorSprite();
            _secondaryDisplayImage.enabled = _secondaryDisplayImage.overrideSprite;
        }
    }
}