using System;
using Features.Cooldowns;
using Features.Skills;
using UnityEngine;

namespace Features.Character
{
    public class CharacterCooldownsManager : MonoBehaviour
    {
        private GameObject Root;

        private SkillsController m_SkillsController;

        private CooldownsController m_CooldownsController;
        
        private void Start()
        {
            Root = transform.root.gameObject;

            m_CooldownsController = Root.GetComponentInChildren<CooldownsController>();

            m_SkillsController = Root.GetComponentInChildren<SkillsController>();

            if (!m_SkillsController) return;
            
            m_SkillsController.OnSkillActivated += SetSkillCooldown;
        }

        private void SetSkillCooldown(SkillActivationContext context, SkillActivationResult result, float cooldown)
        {
            if (!result.IsSuccess) return;
            
            m_CooldownsController.AddCooldown(context.Skill, cooldown);
        }
    }
}