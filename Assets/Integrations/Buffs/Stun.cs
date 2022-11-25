using Features.Buffs;
using Features.Conditions;
using Integrations.StatusEffects;
using UnityEngine;

namespace Integrations.Buffs
{
    public static class Stun
    {

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffMetadata metadata = new BuffMetadata(nameof(Stun), 5f);
            BuffMetadataRegistry.Register(metadata);

            BuffImplementation implementation = new(nameof(Stun), OnReceive, OnRemove);
            BuffImplementationRegistry.Register(implementation);
        }

        private static void OnReceive(BuffActivationPayload buffActivationPayload)
        {
            var se = buffActivationPayload.Target.GetComponentInChildren<StatusEffectsController>();

            if (!se) return;

            se.AddStatusEffect(
                new StatusEffectAddPayload(
                    new StatusEffectMetadata(nameof(StunStatusEffect))));

        }

        private static void OnRemove(BuffActivationPayload buffActivationPayload)
        {
            var se = buffActivationPayload.Target.GetComponentInChildren<StatusEffectsController>();

            if (!se) return;

            se.RemoveStatusEffect(
                new StatusEffectRemovePayload(
                    new StatusEffectMetadata(nameof(StunStatusEffect))));
        }
    }
}