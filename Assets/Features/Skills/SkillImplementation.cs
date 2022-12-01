using System;

namespace Features.Skills
{
    public class SkillImplementation
    {
        public Action<SkillActivationContext> OnReceive;
        public Action<SkillActivationContext> OnActivation;
        public Action<SkillActivationContext> OnRemove;
    }
}