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
        
        public bool ChanneledSkill => ChannelingTime > 0f;
        
        public SkillMetadata(string implementationName, string referenceName, string displayName, float channelingTime, float cooldown, SkillTarget target, SkillFlags flags)
        {
            DisplayName = displayName;
            ChannelingTime = channelingTime;
            Cooldown = cooldown;
            Target = target;
            Flags = flags;
            ImplementationName = implementationName;
            ReferenceName = referenceName;
        }

        public SkillInstance MakeInstance => new(this,
            SkillImplementationRegistry.Implementations[ImplementationName]);
    }
}