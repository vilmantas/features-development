using System.Collections.Concurrent;

namespace Features.Actions
{
    public static class ActionImplementationRegistry
    {
        public static readonly ConcurrentDictionary<string, ActionImplementation> Implementations = new();
    }
}