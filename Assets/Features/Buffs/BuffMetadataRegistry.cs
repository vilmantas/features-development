using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Buffs
{
    public static class BuffMetadataRegistry
    {
        public static IReadOnlyDictionary<string, BuffBase> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, BuffBase> m_Implementations =
            new();

        public static void Register(BuffBase buffMetadata)
        {
            if (Implementations.ContainsKey(buffMetadata.Name)) return;

            m_Implementations.TryAdd(buffMetadata.Name, buffMetadata);
        }
    }
}