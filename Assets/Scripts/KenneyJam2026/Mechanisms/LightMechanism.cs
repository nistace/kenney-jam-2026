using KenneyJam2026.Interactables;
using UnityEngine;

namespace KenneyJam2026.Mechanisms
{
    public class LightMechanism : MonoBehaviour
    {
        [SerializeField] private ToggleInteractable _interactable;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private int _materialIndex;
        [SerializeField] private Light _light;
        [SerializeField] private Material _offMaterial;
        [SerializeField] private Material _onMaterial;

        private void OnEnable()
        {
            _interactable.OnIsOnChanged += HandleInteractableIsOnChanged;

            Refresh();
        }

        private void OnDisable()
        {
            _interactable.OnIsOnChanged -= HandleInteractableIsOnChanged;
        }

        private void HandleInteractableIsOnChanged(bool isOn) => Refresh();

        private void Refresh()
        {
            _light.enabled = _interactable.IsOn;
            var materials = _renderer.materials;
            materials[_materialIndex] = _interactable.IsOn ? _onMaterial : _offMaterial;
            _renderer.materials = materials;    
        }
    }
}