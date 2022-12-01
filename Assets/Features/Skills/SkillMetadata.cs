namespace Features.Skills
{
    public class SkillMetadata
    {
        public readonly string InternalName;

        public readonly string DisplayName;

        public SkillMetadata(string internalName, string displayName)
        {
            DisplayName = displayName;
            InternalName = internalName;
        }
    }
}