using System;
using Features.Health;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthPillController : MonoBehaviour
    {
        public int HealValue = 1;

        private bool m_IsExpended;
        
        private void OnTriggerEnter(Collider other)
        {
            if (m_IsExpended) return;
            
            var character = other.transform.root.GetComponent<CharacterController>();

            if (character == null) return;

            var health = character.GetComponentInChildren<HealthController>();

            if (health == null) return;
            
            m_IsExpended = true;
            
            health.Heal(HealValue);

            Destroy(gameObject);
        }
    }
}