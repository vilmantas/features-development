using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Skills
{
    public class SkillMetadataRegistry
    {
        public static IReadOnlyDictionary<string, SkillMetadata> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, SkillMetadata> m_Implementations =
            new();

        public static void Register(SkillMetadata skill)
        {
            if (Implementations.ContainsKey(skill.ReferenceName)) return;

            m_Implementations.TryAdd(skill.ReferenceName, skill);
        }
    }
}