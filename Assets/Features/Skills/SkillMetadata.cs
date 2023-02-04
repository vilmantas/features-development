using System.Collections.Generic;
using System.Linq;

namespace Features.Skills
{
    public class SkillMetadata
    {
        public readonly string ImplementationName;
        
        public readonly string ReferenceName;

        public readonly string DisplayName;

        public readonly float ChannelingTime;

        public readonly float Cooldown;

        public readonly SkillTarget Target;

        public readonly SkillFlags Flags;

        public readonly Dictionary<string, ExtraData> Extras;
        
        public bool ChanneledSkill => ChannelingTime > 0f;

        public SkillMetadata(string implementationName, string referenceName, string displayName,
            float channelingTime, float cooldown, SkillTarget target, SkillFlags flags,
            IEnumerable<ExtraData> extras)
        {
            DisplayName = displayName;
            ChannelingTime = channelingTime;
            Cooldown = cooldown;
            Target = target;
            Flags = flags;
            ImplementationName = implementationName;
            ReferenceName = referenceName;
            Extras = extras.ToDictionary(x => x.Title, x => x);
        }

        public SkillInstance MakeInstance => new(this,
            SkillImplementationRegistry.Implementations[ImplementationName]);

        public bool HasFlag(SkillFlags flag)
        {
            return (Flags & flag) == flag;
        }
    }
}