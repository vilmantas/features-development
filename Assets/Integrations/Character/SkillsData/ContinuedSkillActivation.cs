using Features.Skills;
using UnityEngine;

namespace Features.Character.SkillsData
{
    public class ContinuedSkillActivation : SkillActivationContext
    {
        public ContinuedSkillActivation Prev { get; set; }

        public bool IsOfType<T>() where T: ContinuedSkillActivation
        {
            if (this is T t) return true;
                
            return Prev != null && Prev.IsOfType<T>();
        }

        public ContinuedSkillActivation(SkillActivationContext ctx) : base(ctx)
        {
            Prev = ctx as ContinuedSkillActivation;
        }
    }
}