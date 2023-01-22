using Features.Conditions;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class AttackingStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(AttackingStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
        }
    }
}