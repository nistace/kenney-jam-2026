using KenneyJam2026.NPCs;
using UnityEngine;

namespace KenneyJam2026.Scores
{
    public class ScoreViewer : MonoBehaviour
    {
        [SerializeField] private NpcManager _npcManager;
        [SerializeField] private MeshRenderer[] _stars;
        [SerializeField] private Material _inactiveStarMaterial;
        [SerializeField] private Material _activeStarMaterial;

        private void Start()
        {
            RefreshActiveStars();

            _npcManager.OnHighestNpcLevelCompletedIncreased += HandleHighestNpcLevelCompletedIncreased;
        }

        private void OnDestroy()
        {
            _npcManager.OnHighestNpcLevelCompletedIncreased -= HandleHighestNpcLevelCompletedIncreased;
        }

        private void HandleHighestNpcLevelCompletedIncreased(int newBest) => RefreshActiveStars();

        private void RefreshActiveStars()
        {
            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i].material = i < _npcManager.HighestNpcLevelCompleted ? _activeStarMaterial : _inactiveStarMaterial;
            }
        }
    }
}