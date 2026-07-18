using UnityEngine;

namespace KenneyJam2026.Interactables
{
    [CreateAssetMenu]
    public class InteractableType : ScriptableObject
    {
        [SerializeField] private Sprite[] _uiSprites;
        [SerializeField] private float _uiSpritesSpeed = 2;
        [SerializeField] private Sprite[] _uiSecondarySprites;
        [SerializeField] private float _uiSecondarySpritesSpeed = 2;

        public bool HasCursorSprites => _uiSprites is { Length: > 0 };

        public Sprite GetTimedCursorSprite() => GetTimedSprite(_uiSprites, _uiSpritesSpeed);
        public Sprite GetTimedSecondaryCursorSprite() => GetTimedSprite(_uiSecondarySprites, _uiSecondarySpritesSpeed);

        private static Sprite GetTimedSprite(Sprite[] _series, float speed)
        {
            if (_series == null || _series.Length == 0) return default;

            return _series[Mathf.Clamp(Mathf.FloorToInt(Time.time * speed % _series.Length), 0, _series.Length - 1)];
        }
    }
}