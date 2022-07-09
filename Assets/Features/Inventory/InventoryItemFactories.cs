using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public static class InventoryItemFactories
    {
        private static readonly ConcurrentDictionary<string, Func<StorageData, StorageData>>
            m_FactoryDictionary = new();

        public static IReadOnlyDictionary<string, Func<StorageData, StorageData>> Registry => m_FactoryDictionary;

        public static void Register(Type type, Func<StorageData, StorageData> metadata)
        {
            var typeName = type.ToString();

            if (m_FactoryDictionary.TryGetValue(typeName, out var _)) return;

            m_FactoryDictionary.TryAdd(typeName, metadata);
        }

        public static StorageData MakeItem(StorageData original)
        {
            var type = original.Parent.GetType().ToString();

            if (!InventoryItemFactories.Registry.TryGetValue(type, out var factory))
            {
                throw new NotSupportedException($"Factory not registered for type {type}");
            }

            return factory(original);
        }
    }
}