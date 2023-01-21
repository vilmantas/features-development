using Features.Actions;
using Features.Conditions;
using Features.Movement;
using Features.OverheadParticles;
using Features.WeaponAnimationConfigurations;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public static class StunStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation = new(nameof(StunStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);

            var particles = Resources.Load<ParticleSystem>("Particles/Stun");

            if (!particles) return;
            
            OverheadsRegistry.Register(nameof(StunStatusEffect), particles);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
            var actionsController = payload.Target.GetComponentInChildren<ActionsController>();

            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();

            var hitboxController =
                payload.Target.GetComponentInChildren<HitboxAnimationController>();

            if (hitboxController)
            {
                hitboxController.Interrupt();
            }

            StatusEffectPresets.DisableActivity(actionsController,
                nameof(StunStatusEffect));
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            var actionsController = payload.Target.GetComponentInChildren<ActionsController>();

            StatusEffectPresets.RemoveConditionHandler(actionsController,
                nameof(StunStatusEffect));
        }
    }
}