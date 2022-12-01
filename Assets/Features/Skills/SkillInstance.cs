namespace Features.Skills
{
    public class SkillInstance
    {
        public readonly SkillMetadata Metadata;

        public readonly SkillImplementation Implementation;

        public SkillInstance(SkillMetadata metadata, SkillImplementation implementation)
        {
            Metadata = metadata;
            Implementation = implementation;
        }
    }
}