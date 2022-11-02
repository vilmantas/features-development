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
            Debug.Log("RandomCondition applied");

            var character = payload.Target.GetComponent<Modules.Character>();
            
            character.m_MovementController.Stop();
            
            character.m_MovementController.OnBeforeMove += OnBeforeMove;
            
            character.m_EquipmentController.OnBeforeEquip += OnBeforeEquip;
        }

        private static void OnBeforeEquip(EquipRequest obj)
        {
            obj.PreventDefault = true;
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            Debug.Log("RandomCondition removed");

            var character = payload.Target.GetComponent<Modules.Character>();
            
            character.m_MovementController.OnBeforeMove -= OnBeforeMove;
            
            character.m_EquipmentController.OnBeforeEquip -= OnBeforeEquip;
        }

        private static void OnBeforeMove(MoveActionData obj)
        {
            obj.PreventDefault = true;
        }
    }
}