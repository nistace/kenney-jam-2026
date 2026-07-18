using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class MilkPouringReceiver : MonoBehaviour
    {
        [SerializeField] private MilkContainer _container;

        public bool Feed(float pouredAmount) => Mathf.Approximately(_container.Add(pouredAmount), pouredAmount);
    }
}