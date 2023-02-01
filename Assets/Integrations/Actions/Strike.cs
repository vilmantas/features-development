using System;
using System.Collections.Generic;
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
            implementation.ActivationWithResultAction = OnActivationWithResult;
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        public static StrikeActionPayload MakePayload(GameObject source)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Strike)), source);

            return new StrikeActionPayload(basePayload);
        }

        private static void OnActivation(
            ActionActivationPayload payload)
        {
            OnActivationWithResult(payload);
        }
        
        private static ActionActivationResult OnActivationWithResult(ActionActivationPayload payload)
        {
            if (payload is not StrikeActionPayload strikePayload)
            {
                throw new ArgumentException("Invalid type of payload passed to strike action");
            }
            
            var combatController = payload.Target.GetComponentInChildren<CombatController>();

            var result = combatController.Strike(strikePayload.StrikeId);

            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.Stop();
            
            return new ActionActivationResult(result);
        }

        public class StrikeActionPayload : ActionActivationPayload
        {
            public readonly Guid StrikeId;
            
            public StrikeActionPayload(ActionActivationPayload original) : base(original.Action,
                original.Source, original.Target)
            {
                StrikeId = Guid.NewGuid();
            }
        }
    }
}