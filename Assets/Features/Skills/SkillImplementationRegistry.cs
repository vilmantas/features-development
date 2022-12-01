using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Skills
{
    public class SkillImplementationRegistry
    {
        public static IReadOnlyDictionary<string, SkillImplementation> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, SkillImplementation> m_Implementations =
            new();

        public static void Register(string name, SkillImplementation skill)
        {
            if (Implementations.ContainsKey(name)) return;

            m_Implementations.TryAdd(name, skill);
        }
    }
}