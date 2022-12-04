using System;
using System.Collections.Generic;
using Features.Skills;
using TMPro;
using UnityEngine;

namespace Integrations.Skills.UI
{
    public class SkillsUIController : MonoBehaviour
    {
        private SkillsController m_Source;

        public TextMeshProUGUI Text;

        public SkillUIDataController SkillPrefab;

        public List<SkillUIDataController> UIDatas; 
        
        public void Initialize(SkillsController source)
        {
            m_Source = source;
            
            m_Source.OnSkillAdded += _ => UpdateUI();
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (var skillUIDataController in UIDatas)
            {
                Destroy(skillUIDataController);
            }

            foreach (var skillMetadata in m_Source.Skills)
            {
                var instance = Instantiate(SkillPrefab, transform);
                
                instance.Initialize(skillMetadata);
            }
        }
    }
}