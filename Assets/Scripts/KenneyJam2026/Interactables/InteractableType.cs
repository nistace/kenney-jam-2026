using UnityEngine;

namespace KenneyJam2026.Interactables
{
    [CreateAssetMenu]
    public class InteractableType : ScriptableObject
    {
        [SerializeField] private Sprite[] _uiSprites;
        [SerializeField] private float _uiSpritesSpeed = 2;

        public bool HasCursorSprites => _uiSprites is { Length: > 0 };

        public Sprite GetTimedCursorSprite()
        {
            if (!HasCursorSprites) return default;

            return _uiSprites[Mathf.Clamp(Mathf.FloorToInt(Time.time * _uiSpritesSpeed % _uiSprites.Length), 0, _uiSprites.Length - 1)];
        }
    }
}