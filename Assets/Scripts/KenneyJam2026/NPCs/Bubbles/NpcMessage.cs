using UnityEngine;

namespace KenneyJam2026.NPCs.Bubbles
{
    [CreateAssetMenu]
    public class NpcMessage : ScriptableObject
    {
        [SerializeField] private float[] _delays;
        [SerializeField] private NpcBubble[] _bubbles;

        public float GetDelay(int index) => index < 0 || index >= _delays.Length ? 0f : _delays[index];
        public NpcBubble GetBubble(int index) => _bubbles[index];
        public int BubbleCount => _bubbles.Length;
    }
}