using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<string> PreparedSkills = new();
        
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

            if (IsSkillPrepared(obj)) return;
            
            var command = new ChannelingCommand("skill_" + obj.Metadata.ReferenceName,
                obj.Metadata.CastTime)
            {
                Callback = () => ContinueActivation(obj)
            };

            m_ChannelingController.StartChanneling(command);

            obj.PreventDefault = true;
        }

        private bool IsSkillPrepared(SkillActivationContext context)
        {
            if (!PreparedSkills.Any(x => x.Equals(context.Skill))) return false;

            PreparedSkills.Remove(context.Skill);

            return true;
        }

        private void ContinueActivation(SkillActivationContext obj)
        {
            PreparedSkills.Add(obj.Skill);

            obj.PreventDefault = false;
            
            m_SkillsController.ActivateSkill(obj);
        }
    }
}