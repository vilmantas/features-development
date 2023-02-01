using System;

namespace Features.Skills
{
    public class SkillImplementation
    {
        public Action<SkillActivationContext> OnReceive;
        public readonly Func<SkillActivationContext, SkillActivationResult> OnActivation;
        public Action<SkillActivationContext> OnRemove;

        public SkillImplementation(Func<SkillActivationContext, SkillActivationResult> onActivation)
        {
            OnActivation = onActivation;
        }
    }
}