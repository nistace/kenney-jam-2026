using UnityEngine;

namespace KenneyJam2026.Milk
{
    public class MilkLevelDisplay : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private MilkContainer _container;
        [SerializeField] private Vector3 _localPositionEmpty;
        [SerializeField] private Vector3 _localPositionFull;
        [SerializeField] private float _visibleThresholdRatio = .1f;

        private void Start()
        {
            Refresh();
            _container.OnChargeChanged += HandleMilkChargeChanged;
        }

        private void OnDestroy()
        {
            _container.OnChargeChanged -= HandleMilkChargeChanged;
        }

        private void HandleMilkChargeChanged() => Refresh();

        private void Refresh()
        {
            transform.localPosition = Vector3.Lerp(_localPositionEmpty, _localPositionFull, _container.CurrentChargeRatio);
            _renderer.enabled = _container.CurrentChargeRatio >= _visibleThresholdRatio;
        }
    }
}