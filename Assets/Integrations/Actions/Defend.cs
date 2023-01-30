using System;
using Features.Actions;
using Features.Combat;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Defend
    {
        public static DefendActionPayload MakePayload(GameObject source, bool isDefending)
        {
            var basePaylaod = new ActionActivationPayload(new ActionBase(nameof(Defend)), source);

            return new DefendActionPayload(basePaylaod, isDefending);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Defend), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }
        
        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not DefendActionPayload DefendActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to Defend action");
            }
            
            var combatController = DefendActionPayload.Target.GetComponentInChildren<CombatController>();

            combatController.SetBlocking(DefendActionPayload.IsDefending);
        }
        
        public class DefendActionPayload : ActionActivationPayload
        {
            public bool IsDefending;
            
            public DefendActionPayload(ActionActivationPayload original, bool isDefending) : base(original.Action,
                original.Source, original.Target)
            {
                IsDefending = isDefending;
            }
        }
    }
}