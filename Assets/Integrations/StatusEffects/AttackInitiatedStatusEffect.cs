using Features.Conditions;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class AttackInitiatedStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(AttackInitiatedStatusEffect), OnStatusEffectApplied,
                OnStatusEffectRemoved);
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