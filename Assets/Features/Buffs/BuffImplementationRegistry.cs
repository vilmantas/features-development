using System.Collections.Concurrent;

namespace Features.Buffs
{
    public static class BuffImplementationRegistry
    {
        public static ConcurrentDictionary<string, BuffImplementation> Implementations;
    }
}