using UnityEngine;

namespace KenneyJam2026.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip[] _clips;

        public float Pitch
        {
            get => _source.pitch;
            set => _source.pitch = value;
        }

        private void Reset()
        {
            _source = GetComponent<AudioSource>();
        }

        public void Play() => Play(0);

        public void Play(int clipIndex)
        {
            if (clipIndex >= 0 && clipIndex < _clips.Length)
            {
                Play(_clips[clipIndex]);
            }
            else
            {
                Debug.LogError($"Invalid clip index: {clipIndex}");
            }
        }

        public void Play(AudioClip clip)
        {
            if (!clip)
            {
                Debug.LogError($"Invalid clip");
                return;
            }

            _source.PlayOneShot(clip);
        }
    }
}