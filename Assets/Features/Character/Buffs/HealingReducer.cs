using Features.Buffs;
using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace Features.Character.Buffs
{
    public static class HealingReducer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffImplementation implementation = new("HealingReducer", OnReceive, OnRemove, null, null);
            BuffImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnReceive(BuffActivationPayload payload)
        {
            var ctrl = payload.Target.GetComponentInChildren<HealthController>();

            ctrl.OnHealingAttemptedNew += HealingLimiter;
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
            var ctrl = payload.Target.GetComponentInChildren<HealthController>();

            ctrl.OnHealingAttemptedNew -= HealingLimiter;
        }

        private static HealthChangeInterceptedEventArgs HealingLimiter(HealthChangeAttemptedEventArgs args)
        {
            return new HealthChangeInterceptedEventArgs(args, args.Amount / 2);
        }
    }
}