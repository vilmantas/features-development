using UnityEngine;

namespace Features.Conditions
{
    public static class RandomStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(RandomStatusEffect), OnStatusEffectApplied);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnStatusEffectApplied()
        {
            Debug.Log("RandomCondition applied");
        }
    }
}