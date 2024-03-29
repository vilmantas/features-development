using Features.Actions;
using Features.Conditions;
using Features.Movement;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class ChannelingStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(ChannelingStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
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