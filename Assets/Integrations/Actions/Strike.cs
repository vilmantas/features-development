using Features.Actions;
using Features.Combat;
using Features.Movement;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Strike
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Strike), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        private static void OnActivation(ActionActivationPayload payload)
        {
            var combatController = payload.Target.GetComponentInChildren<CombatController>();

            combatController.Strike();

            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();
        }
    }
}