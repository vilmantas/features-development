using System;
using System.Collections.Generic;
using System.Linq;
using Features.Cooldowns;
using Features.Skills;
using TMPro;
using UnityEngine;

namespace Integrations.Skills.UI
{
    public class SkillsUIController : MonoBehaviour
    {
        private SkillsController m_Source;

        private CooldownsController m_CooldownsController;

        public SkillUIDataController SkillPrefab;

        public List<SkillUIDataController> UIDatas; 
        
        public void Initialize(SkillsController source, CooldownsController cooldowns)
        {
            m_Source = source;

            m_CooldownsController = cooldowns;
            
            m_Source.OnSkillAdded += _ => UpdateUI();
            
            m_CooldownsController.OnCooldownReceived += CheckCooldowns;
            
            m_CooldownsController.OnCooldownExpired += CheckCooldowns;
            
            UpdateUI();
        }

        private void CheckCooldowns(ActiveCooldown obj)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (var skillUIDataController in UIDatas)
            {
                Destroy(skillUIDataController.gameObject);
            }
            
            UIDatas.Clear();

            foreach (var skillMetadata in m_Source.Skills)
            {
                var instance = Instantiate(SkillPrefab, transform);
                
                instance.Initialize(skillMetadata);
                
                UIDatas.Add(instance);
            }
            
            SetCooldowns();
        }

        private void SetCooldowns()
        {
            foreach (var data in UIDatas)
            {
                if (!m_CooldownsController.IsOnCooldown(data.Parent.InternalName)) return;

                var cd = m_CooldownsController.ActiveCooldowns.First(x =>
                    x.Name == data.Parent.InternalName);
                
                data.SetCooldown(cd);
            }
        }
    }
}