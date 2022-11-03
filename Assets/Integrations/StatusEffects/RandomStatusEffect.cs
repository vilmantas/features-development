using Features.Character;
using Features.Conditions;
using Features.Equipment;
using Features.Movement;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class RandomStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(RandomStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
            var character = payload.Target.GetComponent<Modules.Character>();
            
            character.m_MovementController.Stop();
            
            StatusEffectPresets.DisableActivity(character, nameof(RandomStatusEffect));
            
            character.m_MovementController.OnBeforeMove += OnBeforeMove;
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            var character = payload.Target.GetComponent<Modules.Character>();
            
            StatusEffectPresets.EnableActivity(character, nameof(RandomStatusEffect));
            
            character.m_MovementController.OnBeforeMove -= OnBeforeMove;
        }

        private static void OnBeforeMove(MoveActionData obj)
        {
            obj.PreventDefault = true;
        }
    }
}