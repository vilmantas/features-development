using System;

namespace Features.Skills
{
    public class SkillImplementation
    {
        public readonly Action<SkillActivationContext> OnReceive;
        public readonly Action<SkillActivationContext> OnActivation;
        public readonly Action<SkillActivationContext> OnRemove;

        public SkillImplementation(Action<SkillActivationContext> onReceive, Action<SkillActivationContext> onActivation, Action<SkillActivationContext> onRemove)
        {
            OnReceive = onReceive;
            OnActivation = onActivation;
            OnRemove = onRemove;
        }
    }
}