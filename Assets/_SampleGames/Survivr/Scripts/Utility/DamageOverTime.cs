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

        private HealthController m_Health;
        
        public void Initialize()
        {
            m_Health = transform.root.GetComponentInChildren<HealthController>();
            
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