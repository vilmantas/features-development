using System;
using Features.Health.Events;
using TMPro;
using UnityEngine;

namespace Features.Health
{
    public class HealthUIControllerLegacy : MonoBehaviour
    {
        private HealthController _source;

        private TextMeshProUGUI m_HealthValueText;
        
        public void Awake()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var text in texts)
            {
                if (text.name != "value") continue;
                
                m_HealthValueText = text;
                break;
            }
        }

        public void Initialize(HealthController healthController)
        {
            _source = healthController;

            m_HealthValueText.text = _source.CurrentHealth.ToString();
            
            _source.OnChange += UpdateHealth;
        }

        private void UpdateHealth(HealthChangeEventArgs obj)
        {
            m_HealthValueText.text = obj.After.ToString();
        }
    }
}