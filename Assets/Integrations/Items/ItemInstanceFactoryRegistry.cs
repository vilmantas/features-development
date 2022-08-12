using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Items
{
    public static class ItemInstanceFactoryRegistry
    {
        private static readonly ConcurrentDictionary<string, Func<object>> m_FactoryDictionary = new();

        public static IReadOnlyDictionary<string, Func<object>> Registry => m_FactoryDictionary;

        public static void RegisterMetadata(Type type, Func<object> metadata)
        {
            var typeName = type.ToString();

            if (m_FactoryDictionary.TryGetValue(typeName, out var _)) return;

            m_FactoryDictionary.TryAdd(typeName, metadata);
        }
    }
}