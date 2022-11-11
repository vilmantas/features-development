using Features.Character;
using Features.Conditions;
using UnityEngine;

namespace Integrations.Character.StatusEffects
{
    public static class DoingActionStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(DoingActionStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
            var character = payload.Target.GetComponent<Modules.Character>();
            
            character.m_MovementController.Stop();
            
            StatusEffectPresets.DisableActivity(character, nameof(DoingActionStatusEffect));
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            var character = payload.Target.GetComponent<Modules.Character>();
            
            StatusEffectPresets.EnableActivity(character, nameof(DoingActionStatusEffect));
        }
    }
}