using UnityEngine;

namespace Features.Character
{
    public class CharacterAnimationHooksController : MonoBehaviour
    {
        private static readonly int s_IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int s_Strike1 = Animator.StringToHash("Strike_1");
        private Animator m_Animator;

        private void Awake()
        {
            var events = transform.root.GetComponentInChildren<CharacterEvents>();

            m_Animator = GetComponent<Animator>();

            if (!events) return;

            events.OnMoving += () => m_Animator.SetBool(s_IsWalking, true);

            events.OnStopped += () => m_Animator.SetBool(s_IsWalking, false);

            events.OnStrike += () => m_Animator.SetTrigger(s_Strike1);
        }
    }
}