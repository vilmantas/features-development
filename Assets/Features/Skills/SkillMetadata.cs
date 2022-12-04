namespace Features.Skills
{
    public class SkillMetadata
    {
        public readonly string InternalName;

        public readonly string DisplayName;

        public readonly float CastTime;

        public readonly float Cooldown;
        
        public SkillMetadata(string internalName, string displayName, float castTime, float cooldown)
        {
            DisplayName = displayName;
            CastTime = castTime;
            Cooldown = cooldown;
            InternalName = internalName;
        }

        public SkillInstance MakeInstance => new(this,
            SkillImplementationRegistry.Implementations[InternalName]);
    }
}