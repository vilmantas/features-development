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
            // var actionsController = payload.Target.GetComponentInChildren<ActionsController>();
            //
            // var movementController = payload.Target.GetComponentInChildren<MovementController>();
            //
            // movementController.Stop();
            //
            // StatusEffectPresets.DisableActivity(actionsController,
            //     nameof(ChannelingStatusEffect));
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            // var actionsController = payload.Target.GetComponentInChildren<ActionsController>();
            //
            // StatusEffectPresets.RemoveConditionHandler(actionsController,
            //     nameof(ChannelingStatusEffect));
        }
    }
}