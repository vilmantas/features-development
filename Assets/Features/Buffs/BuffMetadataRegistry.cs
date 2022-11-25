using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Buffs
{
    public static class BuffMetadataRegistry
    {
        public static IReadOnlyDictionary<string, BuffMetadata> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, BuffMetadata> m_Implementations =
            new();

        public static void Register(BuffMetadata buffMetadata)
        {
            if (Implementations.ContainsKey(buffMetadata.Name)) return;

            m_Implementations.TryAdd(buffMetadata.Name, buffMetadata);
        }
    }
}