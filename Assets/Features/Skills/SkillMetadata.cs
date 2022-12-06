namespace Features.Skills
{
    public class SkillMetadata
    {
        public readonly string ImplementationName;
        
        public readonly string ReferenceName;

        public readonly string DisplayName;

        public readonly float CastTime;

        public readonly float Cooldown;
        
        public SkillMetadata(string implementationName, string referenceName, string displayName, float castTime, float cooldown)
        {
            DisplayName = displayName;
            CastTime = castTime;
            Cooldown = cooldown;
            ImplementationName = implementationName;
            ReferenceName = referenceName;
        }

        public SkillInstance MakeInstance => new(this,
            SkillImplementationRegistry.Implementations[ImplementationName]);
    }
}