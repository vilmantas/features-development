namespace Features.Skills
{
    public class SkillMetadata
    {
        public readonly string ImplementationName;
        
        public readonly string ReferenceName;

        public readonly string DisplayName;

        public readonly float CastTime;

        public readonly float Cooldown;

        public readonly SkillTarget Target;
        
        public bool ChanneledSkill => CastTime > 0f;
        
        public SkillMetadata(string implementationName, string referenceName, string displayName, float castTime, float cooldown, SkillTarget target)
        {
            DisplayName = displayName;
            CastTime = castTime;
            Cooldown = cooldown;
            Target = target;
            ImplementationName = implementationName;
            ReferenceName = referenceName;
        }

        public SkillInstance MakeInstance => new(this,
            SkillImplementationRegistry.Implementations[ImplementationName]);
    }
}