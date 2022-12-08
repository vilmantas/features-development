using System;
using Features.Cooldowns;
using Features.Skills;
using UnityEngine;

namespace Features.Character
{
    public class CharacterChannelingManager : MonoBehaviour
    {
        private GameObject Root;

        private SkillsController m_SkillsController;

        private ChannelingController m_ChannelingController;
        
        private void Start()
        {
            Root = transform.root.gameObject;

            m_ChannelingController = Root.GetComponentInChildren<ChannelingController>();

            m_SkillsController = Root.GetComponentInChildren<SkillsController>();

            if (!m_SkillsController) return;
            
            m_SkillsController.OnBeforeActivation += OnBeforeActivation;
        }

        private void OnBeforeActivation(SkillActivationContext obj)
        {
            if (!obj.Metadata.ChanneledSkill) return;
            
            var command = new ChannelingCommand("skill_" + obj.Metadata.ReferenceName,
                obj.Metadata.CastTime);
                
            m_ChannelingController.StartChanneling(command);

            obj.PreventDefault = true;
        }
    }
}