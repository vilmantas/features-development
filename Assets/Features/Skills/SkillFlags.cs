using System;

namespace Features.Skills
{
    [Flags]
    public enum SkillFlags
    {
        None = 0,
        PreventMovement = 1,
        InterruptableByMovement = 2,
        InterruptableByDamage = 4,
        AllowPartialChannelingCompletion = 8,
        BlockAllActions = 16,
    }
}