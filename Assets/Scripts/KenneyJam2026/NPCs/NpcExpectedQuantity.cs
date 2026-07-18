using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;

namespace KenneyJam2026.NPCs
{
    [CreateAssetMenu]
    public class NpcExpectedQuantity : ScriptableObject
    {
        [SerializeField] private NpcMessage _message;
        [SerializeField] private float _expectedQuantity = 1;
        [SerializeField] private float _errorMargin = .05f;

        public NpcMessage Message => _message;
        public float ExpectedQuantity => _expectedQuantity;
        public float ErrorMargin => _errorMargin;
    }
}