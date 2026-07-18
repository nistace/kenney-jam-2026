using UnityEngine;

namespace KenneyJam2026.NPCs.Bubbles
{
    [CreateAssetMenu]
    public class NpcBubbleConfig : ScriptableObject
    {
        [SerializeField] private float _damp = .1f;
        [SerializeField] private AnimationCurve _appearScaleCurve;
        [SerializeField] private AnimationCurve _disappearScaleCurve;
        [SerializeField] private AnimationCurve _jiggleAngleCurve;
        [SerializeField] private float _maxRotationPerSecond = 50;

        public float Damp => _damp;
        public AnimationCurve AppearScaleCurve => _appearScaleCurve;
        public AnimationCurve DisappearScaleCurve => _disappearScaleCurve;
        public AnimationCurve JiggleAngleCurve => _jiggleAngleCurve;
        public float MaxRotationPerSecond => _maxRotationPerSecond;
    }
}