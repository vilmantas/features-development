using UnityEngine;

namespace Features.Skills
{
    public static class SkillLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void RegisterFactory()
        {
            Skill_SO[] allItems = Resources.LoadAll<Skill_SO>("");

            foreach (var item in allItems)
            {
                var metadata = ToMetadata(item);
                
                SkillMetadataRegistry.Register(metadata);
            }
        }

        private static SkillMetadata ToMetadata(Skill_SO skill)
        {
            return new SkillMetadata(skill.ImplementationName, skill.ReferenceName,
                skill.DisplayName, skill.CastTime, skill.Cooldown);
        }
    }
}