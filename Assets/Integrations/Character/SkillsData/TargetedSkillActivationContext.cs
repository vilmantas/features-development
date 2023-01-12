using Features.Skills;
using UnityEngine;

namespace Features.Character.SkillsData
{
    public class TargetedSkillActivationContext : ContinuedSkillActivation
    {
        public void SetTargetObject(GameObject obj) => TargetObject = obj;

        public void SetTargetLocation(Vector3 loc) => TargetLocation = loc;

        public TargetedSkillActivationContext(SkillActivationContext ctx) : base(ctx)
        {
        }
    }
}