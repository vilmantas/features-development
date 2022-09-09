using UnityEngine;

namespace Features.Character
{
    public class CharacterAnimationHooksController : MonoBehaviour
    {
        private static readonly int s_IsWalking = Animator.StringToHash("IsWalking");
        private Animator m_Animator;

        private void Awake()
        {
            var events = transform.root.GetComponentInChildren<CharacterEvents>();

            m_Animator = GetComponent<Animator>();

            if (!events) return;

            events.OnMoving += () => m_Animator.SetBool(s_IsWalking, true);

            events.OnStopped += () => m_Animator.SetBool(s_IsWalking, false);
        }
    }
}