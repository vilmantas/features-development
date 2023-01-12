using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.WeaponAnimationConfigurations
{
    public class WeaponAnimationConfigurationRegistry
    {
        private static readonly ConcurrentDictionary<string, WeaponAnimations_SO>
            m_FactoryDictionary = new();

        public static IReadOnlyDictionary<string, WeaponAnimations_SO> Registry => m_FactoryDictionary;

        public static void Register(WeaponAnimations_SO metadata)
        {
            if (m_FactoryDictionary.TryGetValue(metadata.Type, out var _)) return;

            m_FactoryDictionary.TryAdd(metadata.Type, metadata);
        }
    }
}