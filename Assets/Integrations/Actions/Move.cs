using System;
using Features.Actions;
using Features.Combat;
using Features.Movement;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Move
    {
        public static MoveActionPayload MakePayload(GameObject source, MoveActionData data)
        {
            var basePaylaod = new ActionActivationPayload(new ActionBase(nameof(Move)), source);

            return new MoveActionPayload(basePaylaod, data);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Move), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not MoveActionPayload moveActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to move action");
            }
            
            var movementController = moveActionPayload.Target.GetComponentInChildren<MovementController>();

            movementController.MoveToLocation(moveActionPayload.Data);
        }
        
        public class MoveActionPayload : ActionActivationPayload
        {
            public readonly MoveActionData Data;

            public MoveActionPayload(ActionActivationPayload original, MoveActionData data) : base(original.Action,
                original.Source, original.Target)
            {
                Data = data;
            }
        }
    }
}