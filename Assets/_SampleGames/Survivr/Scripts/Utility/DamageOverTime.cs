using System;
using System.Collections;
using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class DamageOverTime : MonoBehaviour
    {
        [Min(1)] public int Damage;

        [Range(0.1f, 5f)] public float Interval;

        public int Health;

        private HealthController m_Health;
        
        public void Initialize()
        {
            m_Health = transform.root.GetComponentInChildren<HealthController>();
            
            m_Health.Initialize(Health, Health);
            
            if (m_Health == null) return;
            
            StartCoroutine(TickDamage());
        }

        private IEnumerator TickDamage()
        {
            while (true)
            {
                m_Health.Damage(Damage);

                yield return new WaitForSeconds(Interval);
            }
        }
    }
}