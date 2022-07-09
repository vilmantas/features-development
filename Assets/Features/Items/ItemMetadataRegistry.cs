using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Items
{
    public static class ItemMetadataRegistry
    {
        private static readonly ConcurrentDictionary<string, object> m_MetadataDictionary = new();

        public static IReadOnlyDictionary<string, object> Registry => m_MetadataDictionary;

        public static void RegisterMetadata(object metadata)
        {
            var type = metadata.GetType().ToString();

            if (m_MetadataDictionary.TryGetValue(type, out var _)) return;

            m_MetadataDictionary.TryAdd(type, metadata);
        }
    }
}