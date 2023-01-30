using System;
using System.Linq;
using Features.Actions;
using Features.Buffs;
using Features.Health;
using Features.Inventory;
using Features.Movement;
using Integrations.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class StartWalking
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(StartWalking), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        public static StartWalkingActionPayload MakePayload(GameObject source)
        {
            var basePaylaod = new ActionActivationPayload(new ActionBase(nameof(StartWalking)), source);

            return new StartWalkingActionPayload(basePaylaod);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.SetRunning(false);
        }
    }

    public class StartWalkingActionPayload : ActionActivationPayload
    {
        public StartWalkingActionPayload(ActionActivationPayload original) : base(original.Action,
            original.Source, original.Target)
        {
        }
    }
}