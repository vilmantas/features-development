using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Skills
{
    public class SkillsController : MonoBehaviour
    {
        private List<SkillInstance> m_Skills = new();

        public Action<SkillInstance> OnSkillAdded;

        public Action<SkillInstance> OnSkillRemoved;

        public Action<SkillActivationContext, SkillActivationResult, float> OnSkillActivated;

        public IReadOnlyList<SkillMetadata> Skills => m_Skills.Select(x => x.Metadata).ToList();

        private List<SkillMetadata> m_StartingSkills { get; set; }
        
        public void Initialize(IEnumerable<SkillMetadata> skills)
        {
            if (skills == null) return;

            m_StartingSkills = skills.ToList();
            
            foreach (var skillMetadata in m_StartingSkills)
            {
                Add(skillMetadata);
            }
        }

        public void ActivateSkill(SkillActivationContext context)
        {
            var skillInstance =
                m_Skills.FirstOrDefault(x => x.Metadata.InternalName.Equals(context.Skill));
            
            if (skillInstance == null)
            {
                Debug.Log("Skill " + context.Skill + " missing.");

                return;
            }
            
            var result = skillInstance.Implementation.OnActivation.Invoke(context);
            
            OnSkillActivated?.Invoke(context, result, skillInstance.Metadata.Cooldown);
        }

        public void Add(SkillMetadata metadata)
        {
            if (m_Skills.Any(x => x.Metadata.InternalName == metadata.InternalName)) return;
            
            var instance = metadata.MakeInstance;
            
            m_Skills.Add(instance);

            var ctx = new SkillActivationContext(metadata.InternalName, transform.root.gameObject);
            
            instance.Implementation.OnReceive(ctx);
            
            OnSkillAdded?.Invoke(instance);
        }
        
        public void Remove(string skillToRemove)
        {
            var skill = m_Skills.FirstOrDefault(x => x.Metadata.InternalName.Equals(skillToRemove));

            if (skill == null) return;
            
            if (m_StartingSkills.Any(x => x.InternalName == skill.Metadata.InternalName)) return;

            m_Skills.Remove(skill);

            var ctx = new SkillActivationContext(skill.Metadata.InternalName,
                transform.root.gameObject);
            
            skill.Implementation.OnRemove(ctx);
            
            OnSkillRemoved?.Invoke(skill);
        }
    }
}