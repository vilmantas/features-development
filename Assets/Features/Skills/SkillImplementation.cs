using System;

namespace Features.Skills
{
    public class SkillImplementation
    {
        public readonly Action<SkillActivationContext> OnReceive;
        public readonly Func<SkillActivationContext, SkillActivationResult> OnActivation;
        public readonly Action<SkillActivationContext> OnRemove;

        public SkillImplementation(Func<SkillActivationContext, SkillActivationResult> onActivation)
        {
            OnActivation = onActivation;
        }
    }
}