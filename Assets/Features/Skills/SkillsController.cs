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

        public IReadOnlyList<SkillMetadata> Skills => m_Skills.Select(x => x.Metadata).ToList();

        public void Initialize(IEnumerable<SkillMetadata> skills)
        {
            if (m_Skills == null) return;

            foreach (var skillMetadata in skills)
            {
                Add(skillMetadata);
            }
        }

        public void ActivateSkill(string internalName)
        {
            var skillInstance =
                m_Skills.FirstOrDefault(x => x.Metadata.InternalName.Equals(internalName));

            Debug.Log("Skill " + internalName + " missing.");
            
            if (skillInstance == null) return;
            
            skillInstance.Implementation.OnActivation.Invoke(new SkillActivationContext(transform.root.gameObject));
        }

        public void Add(SkillMetadata metadata)
        {
            if (m_Skills.Any(x => x.Metadata.InternalName == metadata.InternalName)) return;
            
            var instance = metadata.MakeInstance;
            
            m_Skills.Add(instance);

            instance.Implementation.OnReceive(
                new SkillActivationContext(transform.root.gameObject));
            
            OnSkillAdded?.Invoke(instance);
        }
        
        public void Remove(string skillToRemove)
        {
            var skill = m_Skills.FirstOrDefault(x => x.Metadata.InternalName.Equals(skillToRemove));

            if (skill == null) return;

            m_Skills.Remove(skill);
            
            OnSkillRemoved?.Invoke(skill);
        }
    }
}