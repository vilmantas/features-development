using System;
using Features.Combat;
using UnityEngine;

namespace Features.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationHooksController : MonoBehaviour
    {
        private readonly int m_RightArmLayerIndex = 1;
        
        private static readonly int s_Velocity = Animator.StringToHash("Velocity");
        private Animator m_Animator;

        private CharacterEvents m_Events;

        private Modules.Character m_Character;
        private static readonly int IsChanneling = Animator.StringToHash("IsChanneling");

        private void Awake()
        {
            var root = transform.root;
            
            m_Character = root.GetComponent<Modules.Character>();

            m_Events = root.GetComponent<CharacterEvents>();

            m_Animator = GetComponent<Animator>();

            if (!m_Events) return;

            m_Events.OnStrike += anim =>
            {
                m_Animator.SetTrigger(anim);
            };
            
            m_Events.OnChannelingStart += OnChannelingStart;
            
            m_Events.OnChannelingEnd += OnChannelingEnd;
        }

        private void OnChannelingEnd()
        {
            m_Animator.SetBool(IsChanneling, false);
        }

        private void OnChannelingStart(string obj)
        {
            m_Animator.SetBool(IsChanneling, true);
            
            m_Animator.Play("Channeling");
        }

        private void Start()
        {
            m_Character.m_CombatController.OnBlockingStatusChanged += state =>
            {
                m_Animator.SetLayerWeight(m_RightArmLayerIndex, state ? 1 : 0);
                Debug.Log("Stop Block");
            };
        }

        private void Update()
        {
            m_Animator.SetFloat(s_Velocity, m_Events.Velocity.magnitude);
        }

        public void StrikeStart()
        {
            m_Events.OnStrikeStart?.Invoke();
        }

        public void StrikeEnd()
        {
            m_Events.OnStrikeEnd?.Invoke();
        }

        public void ProjectileTrigger()
        {
            m_Events.OnProjectileTrigger?.Invoke();
        }
    }
}