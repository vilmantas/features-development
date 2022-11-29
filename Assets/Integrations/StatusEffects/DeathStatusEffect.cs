using Features.Actions;
using Features.Conditions;
using Features.Movement;
using Features.OverheadParticles;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class DeathStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(DeathStatusEffect),
                OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
            
                        
            var particles = Resources.Load<ParticleSystem>("Particles/Death");

            if (!particles) return;
            
            OverheadsRegistry.Register(nameof(DeathStatusEffect), particles);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
            var actionsController = payload.Target.GetComponentInChildren<ActionsController>();

            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();

            StatusEffectPresets.PreventAllActions(actionsController,
                nameof(DeathStatusEffect));
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            var actionsController = payload.Target.GetComponentInChildren<ActionsController>();

            StatusEffectPresets.RemoveConditionHandler(actionsController,
                nameof(DeathStatusEffect));
        }
    }
}