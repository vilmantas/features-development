using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Integrations.Items
{
    public static class ItemScriptRegistry
    {
        private static readonly ConcurrentDictionary<string, ItemScriptImplementation> m_ScriptRegistry = new();

        public static IReadOnlyDictionary<string, ItemScriptImplementation> Registry => m_ScriptRegistry;

        public static void Register(ItemScriptImplementation implementation)
        {
            var name = implementation.Name;

            if (m_ScriptRegistry.TryGetValue(name, out var _)) return;

            m_ScriptRegistry.TryAdd(name, implementation);
        }
    }
}