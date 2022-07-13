using System.Collections.Concurrent;

namespace Features.Buffs
{
    public static class BuffImplementationRegistry
    {
        public static readonly ConcurrentDictionary<string, BuffImplementation> Implementations = new();
    }
}