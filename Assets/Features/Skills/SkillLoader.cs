using System.Linq;
using UnityEngine;

namespace Features.Skills
{
    public static class SkillLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void RegisterSkills()
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
                skill.DisplayName, skill.ChannelingTime, skill.Cooldown, skill.Target, skill.Flags);
        }
    }
}