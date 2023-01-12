using Features.Skills;
using UnityEngine;

namespace Features.Character.SkillsData
{
    public class ChanneledSkillActivationContext : ContinuedSkillActivation
    {
        public ChannelingItem Result { get; }

        public ChanneledSkillActivationContext(SkillActivationContext ctx, ChannelingItem result) : base(ctx)
        {
            Result = result;
        }
    }
}