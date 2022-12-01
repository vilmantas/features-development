using System;
using Features.Actions;
using Features.Conditions;
using Integrations.StatusEffects;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Die
    {
        public static DieActionPayload MakePayload(GameObject source, GameObject target, string cause)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Die)), source, target);

            var data = new DieActionData() { Cause = cause };
            
            return new DieActionPayload(basePayload, data);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Die), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not DieActionPayload deathActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to death action");
            }

            Debug.Log("Died because" + " " + deathActionPayload.Data.Cause);

            var effects =  payload.Target.GetComponentInChildren<StatusEffectsController>();

            if (!effects) return;
            
            var status = new StatusEffectMetadata(nameof(DeathStatusEffect));

            var p = new StatusEffectAddPayload(status);
        
            effects.AddStatusEffect(p);
        }

        public class DieActionPayload : ActionActivationPayload
        {
            public readonly DieActionData Data;

            public DieActionPayload(ActionActivationPayload original, DieActionData data) : base(
                original.Action,
                original.Source, original.Target, original.Data)
            {
                Data = data;
            }
        }

        public class DieActionData
        {
            public string Cause;
        }
    }
}