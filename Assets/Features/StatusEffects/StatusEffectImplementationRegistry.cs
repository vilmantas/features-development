using System.Collections.Concurrent;

namespace Features.Conditions
{
    public class StatusEffectImplementationRegistry
    {
        public static readonly ConcurrentDictionary<string, StatusEffectImplementation>
            Implementations = new();
    }
}