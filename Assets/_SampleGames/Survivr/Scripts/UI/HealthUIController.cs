using System;
using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthUIController : MonoBehaviour
    {
        private GameObject m_Character;

        private HealthController m_HealthController;

        private SimpleLoadingBarController m_HealthBar;

        private void Awake()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player");

            m_HealthBar = GetComponentInChildren<SimpleLoadingBarController>();

            m_HealthController = m_Character.GetComponentInChildren<HealthController>();
            
            UpdateHealthBar(m_HealthController.CurrentHealth);
            
            m_HealthController.OnChange += OnHealthChanged;
        }

        private void OnHealthChanged(HealthChangeEventArgs obj)
        {
            UpdateHealthBar(obj.After);
        }

        private void UpdateHealthBar(int currentHealth)
        {
            var percent = Mathf.Lerp(m_HealthController.MaxHealth, currentHealth, 100);
            m_HealthBar.SetFillPercent(percent);
        }
    }
}