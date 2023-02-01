using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Cooldowns
{
    public class CooldownsController : MonoBehaviour
    {
        public IReadOnlyList<ActiveCooldown> ActiveCooldowns => m_ActiveCooldowns;

        private List<ActiveCooldown> m_ActiveCooldowns = new();

        public Action<ActiveCooldown> OnCooldownExpired;

        public Action<ActiveCooldown> OnCooldownReceived;

        private void Update()
        {
            Tick(Time.deltaTime);
            
            RemoveExpired();
        }

        public void ReduceCooldown(string activeCooldown, float progress)
        {
            var cd = m_ActiveCooldowns.FirstOrDefault(x => x.Name.Equals(activeCooldown));

            cd?.Tick(progress);
        }

        public void AddCooldown(string title, float duration)
        {
            var activeCooldown = new ActiveCooldown(title, duration);
            
            m_ActiveCooldowns.Add(activeCooldown);
            
            OnCooldownReceived?.Invoke(activeCooldown);
        }

        public bool IsOnCooldown(string title) =>
            m_ActiveCooldowns.FirstOrDefault(x => x.Name.Equals(title)) != null; 

        private void Tick(float secondsPassed)
        {
            foreach (var activeCooldown in m_ActiveCooldowns)
            {
                activeCooldown.Tick(secondsPassed);
            }
        }

        private void RemoveExpired()
        {
            var expiredCooldowns = m_ActiveCooldowns.Where(x => x.IsExpired);

            m_ActiveCooldowns = m_ActiveCooldowns.Where(x => !x.IsExpired).ToList();

            foreach (var cd in expiredCooldowns)
            {
                OnCooldownExpired?.Invoke(cd);
            }
        }
    }
}