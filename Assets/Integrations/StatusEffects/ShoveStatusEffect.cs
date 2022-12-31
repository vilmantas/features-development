using Features.Actions;
using Features.Conditions;
using Features.Movement;
using Features.OverheadParticles;
using UnityEngine;

namespace Integrations.StatusEffects
{
    public class ShoveStatusEffect
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            StatusEffectImplementation implementation =
                new(nameof(ShoveStatusEffect), OnStatusEffectApplied, OnStatusEffectRemoved);
            StatusEffectImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);

            var particles = Resources.Load<ParticleSystem>("Particles/Shove");

            if (!particles) return;
            
            OverheadsRegistry.Register(nameof(ShoveStatusEffect), particles);
        }

        private static void OnStatusEffectApplied(StatusEffectPayload payload)
        {
            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();
            
            var rb = payload.Target.GetComponentInChildren<Rigidbody>();

            var nm = payload.Target.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

            nm.isStopped = true;

            rb.isKinematic = false;
        }

        private static void OnStatusEffectRemoved(StatusEffectPayload payload)
        {
            var rb = payload.Target.GetComponentInChildren<Rigidbody>();

            var nm = payload.Target.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();
            
            nm.isStopped = false;

            rb.isKinematic = true;
        }
    }
}