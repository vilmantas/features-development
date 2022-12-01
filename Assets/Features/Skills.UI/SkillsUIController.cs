using System;
using TMPro;
using UnityEngine;

namespace Features.Skills.UI
{
    public class SkillsUIController : MonoBehaviour
    {
        private SkillsController m_Source;

        public TextMeshProUGUI Text;
        
        public void Initialize(SkillsController source)
        {
            m_Source = source;
            
            m_Source.OnSkillAdded += _ => UpdateUI();
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            var skills = "";
            
            foreach (var skillMetadata in m_Source.Skills)
            {
                skills += skillMetadata.DisplayName + Environment.NewLine;
            }
            
            Text.text = skills;
        }
    }
}