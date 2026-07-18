using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class MilkPouringReceiver : MonoBehaviour
    {
        [SerializeField] private MilkContainer _container;
        
        public void Feed()
        {
            _container.Add(.1f);
        }
    }
}