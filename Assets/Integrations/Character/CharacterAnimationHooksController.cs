using UnityEngine;

namespace Features.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationHooksController : MonoBehaviour
    {
        private readonly int m_RightArmLayerIndex = 1;
        
        private static readonly int s_Strike1 = Animator.StringToHash("Strike_1");
        private static readonly int s_Velocity = Animator.StringToHash("Velocity");
        private Animator m_Animator;

        private CharacterEvents m_Events;

        private void Awake()
        {
            m_Events = transform.root.GetComponent<CharacterEvents>();

            m_Animator = GetComponent<Animator>();

            if (!m_Events) return;

            m_Events.OnStrike += () => m_Animator.SetTrigger(s_Strike1);

            m_Events.OnActivateBlock += () =>
            {
                m_Animator.SetLayerWeight(m_RightArmLayerIndex, 1);
                Debug.Log("Blocking!");
            };

            m_Events.OnDeactivateBlock += () =>
            {
                m_Animator.SetLayerWeight(m_RightArmLayerIndex, 0);
                Debug.Log("Stop Block");
            };
        }

        private void Update()
        {
            m_Animator.SetFloat(s_Velocity, m_Events.Velocity.magnitude);
        }
    }
}