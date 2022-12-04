using System;

namespace Features.Skills
{
    public class SkillImplementation
    {
        public readonly Action<SkillActivationContext> OnReceive;
        public readonly Func<SkillActivationContext, SkillActivationResult> OnActivation;
        public readonly Action<SkillActivationContext> OnRemove;

        public SkillImplementation(Action<SkillActivationContext> onReceive,
            Func<SkillActivationContext, SkillActivationResult> onActivation,
            Action<SkillActivationContext> onRemove)
        {
            OnReceive = onReceive;
            OnActivation = onActivation;
            OnRemove = onRemove;
        }
    }
}