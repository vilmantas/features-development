using System;
using System.Collections;
using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace Features.Health.UI
{
    public class HealthUIController : MonoBehaviour
    {
        private HealthController m_Source;

        private HealthDisplayController m_HealthDisplay;
        
        private void Awake()
        {
            var prefab = Resources.Load<HealthDisplayController>("pref_health_display");

            m_HealthDisplay = Instantiate(prefab, transform);

            m_HealthDisplay.gameObject.SetActive(false);
        }

        public void Initialize(HealthController source)
        {
            m_Source = source;
            
            m_Source.OnChange += OnChange;
        }

        private void OnChange(HealthChangeEventArgs obj)
        {
            m_HealthDisplay.UpdateText(obj.After.ToString(), obj.Source.MaxHealth.ToString());

            StartCoroutine(BlinkHealth());
        }

        private IEnumerator BlinkHealth()
        {
            m_HealthDisplay.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
            
            m_HealthDisplay.gameObject.SetActive(false);
        }
    }
}