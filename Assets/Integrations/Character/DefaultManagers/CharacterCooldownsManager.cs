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
            
            m_SkillsController.OnBeforeActivation += OnBeforeActivation;
        }

        private void OnBeforeActivation(SkillActivationContext obj)
        {
            if (!m_CooldownsController.IsOnCooldown(obj.Skill)) return;

            obj.PreventDefault = true;
        }

        private void SetSkillCooldown(SkillActivationContext context, SkillActivationResult result)
        {
            if (!result.IsSuccess) return;
            
            m_CooldownsController.AddCooldown(context.Skill, context.Metadata.Cooldown);
        }
    }
}