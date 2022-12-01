using UnityEngine;

namespace Features.Skills
{
    public class SkillActivationContext
    {
        public readonly GameObject Source;

        public SkillActivationContext(GameObject source)
        {
            Source = source;
        }
    }
}