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
    public static class StartRunning
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(StartRunning), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        public static StartRunningActionPayload MakePayload(GameObject source)
        {
            var basePaylaod = new ActionActivationPayload(new ActionBase(nameof(StartRunning)), source);

            return new StartRunningActionPayload(basePaylaod);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var movementController = payload.Target.GetComponentInChildren<MovementController>();
            
            movementController.SetRunning(true);
        }
    }

    public class StartRunningActionPayload : ActionActivationPayload
    {
        public StartRunningActionPayload(ActionActivationPayload original) : base(original.Action,
            original.Source, original.Target)
        {
        }
    }
}