using System;
using TMPro;
using UnityEngine;

namespace Features.Health.UI
{
    public class HealthDisplayController : MonoBehaviour
    {
        private TextMeshPro m_Text;

        private void Awake()
        {
            m_Text = GetComponentInChildren<TextMeshPro>();
        }

        public void UpdateText(string currentHp, string totalHp)
        {
            m_Text.text = $"{currentHp}/{totalHp}";
        }
    }
}