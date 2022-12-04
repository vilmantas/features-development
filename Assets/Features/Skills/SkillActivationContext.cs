using UnityEngine;

namespace Features.Skills
{
    public class SkillActivationContext
    {
        public readonly string Skill;
        
        public readonly GameObject Source;

        public SkillActivationContext(string skill, GameObject source)
        {
            Skill = skill;
            Source = source;
        }
    }
}