using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Integrations.Items
{
    public static class ItemScriptRegistry
    {
        private static readonly ConcurrentDictionary<string, ItemScript> m_ScriptRegistry = new();

        public static IReadOnlyDictionary<string, ItemScript> Registry => m_ScriptRegistry;

        public static void Register(ItemScript metadata)
        {
            var name = metadata.Name;

            if (m_ScriptRegistry.TryGetValue(name, out var _)) return;

            m_ScriptRegistry.TryAdd(name, metadata);
        }
    }
}