using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class NpcAnimator : MonoBehaviour
    {
        private static readonly int IsMovingAnimParam = Animator.StringToHash("IsMoving");
        private static readonly int IsKnockingAnimParam = Animator.StringToHash("IsKnocking");

        [SerializeField] private Animator _animator;
        [SerializeField] private Npc _npc;

        private void LateUpdate()
        {
            _animator.SetBool(IsMovingAnimParam, _npc.IsMoving);
            _animator.SetBool(IsKnockingAnimParam, _npc.IsKnocking);
        }
    }
}