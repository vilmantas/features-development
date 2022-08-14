using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthUIController : Manager
    {
        private GameObject m_Character;

        private SimpleLoadingBarController m_HealthBar;

        private HealthController m_HealthController;

        public override void Initialize()
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
            m_HealthBar.SetFillPercent((float) currentHealth / m_HealthController.MaxHealth * 100);
        }
    }
}