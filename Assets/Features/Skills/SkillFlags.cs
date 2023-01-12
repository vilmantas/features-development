using System;

namespace Features.Skills
{
    [Flags]
    public enum SkillFlags
    {
        None,
        PreventMovement,
        InterruptableByMovement,
        InterruptableByDamage,
        AllowPartialChannelingCompletion,
    }
}