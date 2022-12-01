using System;

namespace Features.Skills
{
    public class SkillsCooldownProvider
    {
        public readonly Action GetCooldowns;

        public SkillsCooldownProvider(Action getCooldowns)
        {
            GetCooldowns = getCooldowns;
        }
    }
}