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

        private ChannelingController m_ChannelingController;

        public SkillUIDataController SkillPrefab;

        public List<SkillUIDataController> UIDatas;

        public void Initialize(SkillsController source, CooldownsController cooldowns,
            ChannelingController channeling)
        {
            m_Source = source;

            m_CooldownsController = cooldowns;

            m_ChannelingController = channeling;
            
            m_ChannelingController.OnChannelingStarted += OnChannelingStarted;

            m_ChannelingController.OnChannelingCompleted += OnChannelingStarted;
            
            m_Source.OnSkillAdded += _ => UpdateUI();

            m_Source.OnSkillRemoved += _ => UpdateUI();
            
            m_CooldownsController.OnCooldownReceived += CheckCooldowns;
            
            m_CooldownsController.OnCooldownExpired += CheckCooldowns;
            
            UpdateUI();
        }

        private void OnChannelingStarted(ChannelingItem obj)
        {
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

            int i = 1;

            var skills = m_Source.Skills.Where(x => x.ReferenceName != "Basic Attack");

            var basicAttack = m_Source.Skills.FirstOrDefault(x => x.ReferenceName == "Basic Attack");

            SkillUIDataController instance;

            if (basicAttack != null)
            {
                instance = Instantiate(SkillPrefab, transform);

                instance.Initialize(basicAttack, -1);

                UIDatas.Add(instance);
            }

            foreach (var skillMetadata in skills)
            {
                instance = Instantiate(SkillPrefab, transform);
                
                instance.Initialize(skillMetadata, i);
                
                UIDatas.Add(instance);

                i++;
            }
            
            SetCooldowns();
            
            SetChannelings();
        }

        private void SetCooldowns()
        {
            foreach (var data in UIDatas)
            {
                if (!m_CooldownsController.IsOnCooldown(data.Parent.ReferenceName)) continue;

                var cd = m_CooldownsController.ActiveCooldowns.First(x =>
                    x.Name == data.Parent.ReferenceName);
                
                data.SetCooldown(cd);
            }
        }

        private void SetChannelings()
        {
            foreach (var skillUIDataController in UIDatas)
            {
                var channeling = m_ChannelingController.CurrentlyChanneling.FirstOrDefault(x =>
                    x.Title.Replace("skill_",
                        "").Equals(skillUIDataController.Parent.ReferenceName));
                
                skillUIDataController.SetBlock(channeling != null);
            }
        }
    }
}