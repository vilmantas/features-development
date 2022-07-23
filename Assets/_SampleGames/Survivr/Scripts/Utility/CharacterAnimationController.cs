using System;
using UnityEngine;

namespace _SampleGames.Survivr.Scripts.Utility
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator m_Animator;

        private CharacterController m_CharacterController;
        
        private void Start()
        {
            m_Animator = GetComponent<Animator>();

            m_CharacterController = transform.root.GetComponent<CharacterController>();

            m_CharacterController.OnStartedMoving += () => m_Animator.SetBool("IsMoving", true);
            
            m_CharacterController.OnStopped += () => m_Animator.SetBool("IsMoving", false);
        }
    }
}