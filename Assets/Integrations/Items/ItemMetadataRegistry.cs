using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Items
{
    public static class ItemMetadataRegistry
    {
        private static readonly ConcurrentDictionary<string, ItemMetadata> m_MetadataDictionary = new();

        public static IReadOnlyDictionary<string, ItemMetadata> Registry => m_MetadataDictionary;

        public static void Register(ItemMetadata metadata)
        {
            var name = metadata.Name;

            if (m_MetadataDictionary.TryGetValue(name, out var _)) return;

            m_MetadataDictionary.TryAdd(name, metadata);
        }
    }
}