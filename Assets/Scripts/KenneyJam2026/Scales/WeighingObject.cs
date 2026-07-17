using UnityEngine;

namespace KenneyJam2026.Scales
{
    public class WeighingObject : MonoBehaviour
    {
        [SerializeField] private float _weight = 1;

        public float Weight => _weight;
    }
}