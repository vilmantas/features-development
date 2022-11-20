using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Features.Buffs
{
    public static class BuffImplementationRegistry
    {
        public static IReadOnlyDictionary<string, BuffImplementation> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, BuffImplementation> m_Implementations =
            new();

        public static void Register(BuffImplementation buffImplementation)
        {
            if (Implementations.ContainsKey(buffImplementation.Name)) return;

            m_Implementations.TryAdd(buffImplementation.Name, buffImplementation);
        }
    }
}