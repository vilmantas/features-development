using Features.Actions;
using Features.Buffs;
using Features.Conditions;
using Integrations.StatusEffects;
using UnityEngine;

namespace Integrations.Buffs
{
    public class Shove
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffImplementation implementation = new("Shove", OnReceive, OnRemove, OnTick, OnDurationReset);
            BuffImplementationRegistry.Register(implementation);
        }

        private static void OnReceive(BuffActivationPayload payload)
        {
            payload.Buff.State = new ShoveState(description: "");

            var se = payload.Target.GetComponentInChildren<StatusEffectsController>();

            if (!se) return;
            
            se.AddStatusEffect(
                new StatusEffectAddPayload(
                    new StatusEffectMetadata(nameof(ShoveStatusEffect))));
                
            se.AddStatusEffect(
                new StatusEffectAddPayload(
                    new StatusEffectMetadata(nameof(StunStatusEffect))));
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
            var se = payload.Target.GetComponentInChildren<StatusEffectsController>();

            if (!se) return;
            
            se.RemoveStatusEffect(
                new StatusEffectRemovePayload(
                    new StatusEffectMetadata(nameof(ShoveStatusEffect))));
                
            se.RemoveStatusEffect(
                new StatusEffectRemovePayload(
                    new StatusEffectMetadata(nameof(StunStatusEffect))));
        }

        private static void OnTick(BuffActivationPayload payload)
        {
        }

        private static void OnDurationReset(BuffActivationPayload payload)
        {
        }

        private class ShoveState : IBuffState
        {
            public string Description { get; }

            public ShoveState(string description)
            {
                Description = description;
            }
        }
    }
}